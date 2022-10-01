﻿using SoulsLike.Extentions;
using UnityEngine;

namespace SoulsLike
{
	public class PlayerCombatSystem : MonoBehaviour
	{
		private AnimatorHandler _animatorHandler = default;
		private WeaponSlotManager _weaponSlotManager = default;
		private PlayerInventory _playerInventory = default;

		private string _lastAttack = default;
		private bool _comboFlag = default;
		private bool _isTwoHanded = default;

		private void OnEnable()
		{
			this.AddListener<RightWeaponAttack>(OnRightWeaponAttack);
			this.AddListener<ToggleTwoHandEvent>(OnToggleTwoHand);
		}

		private void OnDisable()
		{
			this.RemoveListener<RightWeaponAttack>(OnRightWeaponAttack);
			this.RemoveListener<ToggleTwoHandEvent>(OnToggleTwoHand);
		}

		public void Init(PlayerInventory playerInventory, AnimatorHandler animatorHandler, WeaponSlotManager weaponSlotManager)
		{
			_playerInventory = playerInventory;
			_animatorHandler = animatorHandler;
			_weaponSlotManager = weaponSlotManager;
		}

		private void OnRightWeaponAttack(RightWeaponAttack eventInfo)
		{
			WeaponItem weapon = _playerInventory.RightWeapon;

			if(eventInfo.canDoCombo)
			{
				_comboFlag = true;
				HandleWeaponCombo(weapon);
				_comboFlag = false;
			}
			else
			{
				if(eventInfo.isInteracting) return;

				if(eventInfo.lightAttackInput) HandleAttack(weapon);
				else if(eventInfo.heavyAttackInput) HandleAttack(weapon, true);
			}
		}

		private void OnToggleTwoHand(ToggleTwoHandEvent _)
		{
			_isTwoHanded = !_isTwoHanded;
			WeaponItem rightWeapon = _playerInventory.RightWeapon;
			WeaponItem leftWeapon = _playerInventory.LeftWeapon;

			if(_isTwoHanded)
			{
				_animatorHandler.PlayTargetAnimation(rightWeapon.TwoHandAnimation, false);
				this.TriggerEvent(new WeaponLoadEvent(leftWeapon, false, true));
			}
			else
			{
				_animatorHandler.PlayTargetAnimation(AnimationNameBase.BothArmsEmpty, false);
				this.TriggerEvent(new WeaponLoadEvent(rightWeapon, false));
				this.TriggerEvent(new WeaponLoadEvent(leftWeapon, true));
			}
		}

		private void HandleWeaponCombo(WeaponItem weapon)
		{
			if(!_comboFlag) return;
			_animatorHandler.DisableCombo();

			if(_isTwoHanded)
			{
				if(_lastAttack == weapon.TwoHandedLightAttack01) _animatorHandler.PlayTargetAnimation(weapon.TwoHandedLightAttack02, true);
				else if(_lastAttack == weapon.TwoHandedHeavyAttack01) _animatorHandler.PlayTargetAnimation(weapon.TwoHandedHeavyAttack02, true);
			}
			else
			{
				if(_lastAttack == weapon.OneHandedLightAttack01) _animatorHandler.PlayTargetAnimation(weapon.OneHandedLightAttack02, true);
				else if(_lastAttack == weapon.OneHandedHeavyAttack01) _animatorHandler.PlayTargetAnimation(weapon.OneHandedHeavyAttack02, true);
			}
		}

		private void HandleAttack(WeaponItem weapon, bool isHeavy = false)
		{
			string attackAnimation = _isTwoHanded ? isHeavy ? weapon.TwoHandedHeavyAttack01 : weapon.TwoHandedLightAttack01 :
				isHeavy ? weapon.OneHandedHeavyAttack01 : weapon.OneHandedLightAttack01;

			_weaponSlotManager.SetAttackingWeapon(weapon);
			_animatorHandler.PlayTargetAnimation(attackAnimation, true);
			_lastAttack = _isTwoHanded ? weapon.TwoHandedLightAttack01 : weapon.OneHandedLightAttack01;
		}
	}
}