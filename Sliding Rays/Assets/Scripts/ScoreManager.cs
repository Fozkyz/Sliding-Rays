using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
	public GameObject highscore_entry_prefab;
	public Transform highscore_parent;
	Highscores highscores;
	private void Awake()
	{
		string jsonString = PlayerPrefs.GetString("highscoreTable");
		highscores = JsonUtility.FromJson<Highscores>(jsonString);
		if (highscores == null)
		{
			highscores = new Highscores();
		}
	}

	public void DisplayHighscores()
	{
		for (int i = 0; i < highscores.highscores.Length; i++)
		{
			RectTransform new_highscore_entry = Instantiate(highscore_entry_prefab, highscore_parent).GetComponent<RectTransform>();
			if (new_highscore_entry != null)
			{
				foreach(TextMeshProUGUI ui in new_highscore_entry.GetComponentsInChildren<TextMeshProUGUI>())
				{
					if (ui.name == "Rank_UI")
					{
						switch (i+1)
						{
							case 1: ui.text = "1st"; break;
							case 2: ui.text = "2nd"; break;
							case 3: ui.text = "3rd"; break;
							default: ui.text = (i + 1).ToString() + "th"; break;
						}
					}
					else if (ui.name == "Name_UI")
					{
						ui.text = highscores.highscores[i].name;
					}
					else if (ui.name == "Score_UI")
					{
						ui.text = highscores.highscores[i].score.ToString();
					}
				}
				new_highscore_entry.transform.localPosition = new Vector3(0, 45 - 20 * i, 0);
			}
		}
	}

	public void AddHighscoreEntry(string name, int score)
	{
		HighscoreEntry new_highscore = new HighscoreEntry { name = name, score = score };
		if (highscores.highscores != null)
		{
			int j = 0;
			while (j < highscores.highscores.Length && highscores.highscores[j] != null)
			{
				j++;
			}
			if (score >= highscores.highscores[j-1].score)
			{
				int i = highscores.highscores.Length - 2;
				while (i >= 0 && score >= highscores.highscores[i].score)
				{
					highscores.highscores[i+1] = highscores.highscores[i];
					i--;
				}
				highscores.highscores[i + 1] = new_highscore;
			}
		}
		else
		{
			highscores.highscores = new HighscoreEntry[10];
			highscores.highscores[0] = new_highscore;
		}		
		SaveHighscores(highscores);
	}

	private void SaveHighscores(Highscores highscores)
	{
		string jsonString = JsonUtility.ToJson(highscores);
		PlayerPrefs.SetString("highscoreTable", jsonString);
		PlayerPrefs.Save();
	}

	public int GetHighscore()
	{
		if (highscores.highscores != null)
		{
			return highscores.highscores[0].score;
		}
		else
		{
			return 0;
		}
	}

	private class Highscores
	{
		public HighscoreEntry[] highscores;
	}

	[System.Serializable]
	private class HighscoreEntry
	{
		public string name = "XXX";
		public int score = 0;
	}
}
