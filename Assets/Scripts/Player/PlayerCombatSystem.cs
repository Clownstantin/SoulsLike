using SoulsLike.Extentions;
using UnityEngine;

namespace SoulsLike.Player
{
	public class PlayerCombatSystem : MonoBehaviour, IEventListener, IEventSender
	{
		private PlayerAnimatorHandler _playerAnimatorHandler = default;
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

		public void Init(PlayerInventory playerInventory, PlayerAnimatorHandler playerAnimatorHandler, WeaponSlotManager weaponSlotManager)
		{
			_playerInventory = playerInventory;
			_playerAnimatorHandler = playerAnimatorHandler;
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
				_playerAnimatorHandler.PlayTargetAnimation(rightWeapon.TwoHandAnimation, false);
				this.TriggerEvent(new WeaponLoadEvent(leftWeapon, false, true));
			}
			else
			{
				_playerAnimatorHandler.PlayTargetAnimation(AnimationNameBase.BothArmsEmpty, false);
				this.TriggerEvent(new WeaponLoadEvent(rightWeapon, false));
				this.TriggerEvent(new WeaponLoadEvent(leftWeapon, true));
			}
		}

		private void HandleWeaponCombo(WeaponItem weapon)
		{
			if(!_comboFlag) return;
			_playerAnimatorHandler.DisableCombo();

			if(_isTwoHanded)
			{
				if(_lastAttack == weapon.TwoHandedLightAttack01) _playerAnimatorHandler.PlayTargetAnimation(weapon.TwoHandedLightAttack02, true);
				else if(_lastAttack == weapon.TwoHandedHeavyAttack01) _playerAnimatorHandler.PlayTargetAnimation(weapon.TwoHandedHeavyAttack02, true);
			}
			else
			{
				if(_lastAttack == weapon.OneHandedLightAttack01) _playerAnimatorHandler.PlayTargetAnimation(weapon.OneHandedLightAttack02, true);
				else if(_lastAttack == weapon.OneHandedHeavyAttack01) _playerAnimatorHandler.PlayTargetAnimation(weapon.OneHandedHeavyAttack02, true);
			}
		}

		private void HandleAttack(WeaponItem weapon, bool isHeavy = false)
		{
			string attackAnimation = _isTwoHanded ? isHeavy ? weapon.TwoHandedHeavyAttack01 : weapon.TwoHandedLightAttack01 :
				isHeavy ? weapon.OneHandedHeavyAttack01 : weapon.OneHandedLightAttack01;

			_weaponSlotManager.SetAttackingWeapon(weapon);
			_playerAnimatorHandler.PlayTargetAnimation(attackAnimation, true);
			_lastAttack = _isTwoHanded ? weapon.TwoHandedLightAttack01 : weapon.OneHandedLightAttack01;
		}
	}
}