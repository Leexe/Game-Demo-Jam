using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
	[SerializeField]
	private LayerMask _targetLayerMask;

	[SerializeField]
	private GameObject _visualGameObject;

	[SerializeField]
	private GameObject _arrow;

	[SerializeField]
	private string _itemId;

	[SerializeField]
	private string _questId;

	private bool _isPickedUp;

	private void OnTriggerEnter(Collider other)
	{
		if ((_targetLayerMask.value & (1 << other.gameObject.layer)) != 0 && !_isPickedUp)
		{
			GameManager.Instance.TriggerOnItemPickUp(_itemId);
			QuestManager.Instance.CompleteQuest(_questId);
			_visualGameObject.SetActive(false);
			if (_arrow != null)
			{
				_arrow.SetActive(false);
			}
			_isPickedUp = true;
		}
	}

	// private void OnNewLevel(int level)
	// {
	// 	if (_respawnOnNewLevel)
	// 	{
	// 		_isPickedUp = false;
	// 		_visualGameObject.SetActive(true);
	// 		if (_arrow != null)
	// 		{
	// 			_arrow.SetActive(true);
	// 		}
	// 	}
	// }

	private void OnResetRun()
	{
		_isPickedUp = false;
		_visualGameObject.SetActive(true);
		if (_arrow != null)
		{
			_arrow.SetActive(true);
		}
	}
}
