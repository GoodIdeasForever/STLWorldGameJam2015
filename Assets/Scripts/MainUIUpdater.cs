using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainUIUpdater : MonoBehaviour
{
	static public MainUIUpdater Instance;

	public Image[] evidenceIcons;
	public Text timeRemainingText;
	public Text levelNumber;

	public Image[] evidenceInventory;
	public Image[] lootInventory;

	void Awake()
	{
		Instance = this;
	}

	void OnDestroy()
	{
		Instance = null;
	}

	void Update()
	{
		if (null == GameState.Instance) return;

		var evidenceCount = GameState.Instance.Evidence.Count;
		for (int i = 0; i < GameState.Instance.Player.ItemsInBack.Count; ++i)
		{
			if (GameState.Instance.Player.ItemsInBack[i] == Space.Evidence)
			{
				++evidenceCount;
			}
		}

		for (int i = 0; i < evidenceIcons.Length; ++i)
		{
			evidenceIcons[i].enabled = evidenceCount >= (i + 1);
		}

		timeRemainingText.text = string.Format("{0:D2}:{1:D2}", (int)(GameState.Instance.NextEvidenceDropTime / 60), (int)(GameState.Instance.NextEvidenceDropTime % 60));

		for (int i = 0; i < evidenceInventory.Length; ++i)
		{
			var hasEvidenceInSlot = false;
			if (GameState.Instance.Player.ItemsInBack.Count > i && GameState.Instance.Player.ItemsInBack[i] == Space.Evidence)
			{
				hasEvidenceInSlot = true;
			}
			evidenceInventory[i].enabled = hasEvidenceInSlot;
		}
		for (int i = 0; i < lootInventory.Length; ++i)
		{
			var hasLootInSlot = false;
			if (GameState.Instance.Player.ItemsInBack.Count > i && GameState.Instance.Player.ItemsInBack[i] == Space.Loot)
			{
				hasLootInSlot = true;
			}
			lootInventory[i].enabled = hasLootInSlot;
		}
	}

	void Start()
	{
		levelNumber.text = GameState.Instance.LevelNumber.ToString();
	}
}
