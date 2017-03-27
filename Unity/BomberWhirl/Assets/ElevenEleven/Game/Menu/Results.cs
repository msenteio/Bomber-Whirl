namespace ElevenEleven.Game {
	using UnityEngine;
	using System.Collections;
	using System.Collections.Generic;
	using System.Text;
    using TMPro;

	internal class Results : Singleton<Results> {

        [SerializeField] TextMeshProUGUI winnerText;

		IEnumerator Start() {
            winnerText.gameObject.SetActive(false);

            yield return new WaitForSeconds(1.0f);

			List<PlayerScore> topScores = new List<PlayerScore>();
            foreach (var score in ShallowGames.scores.Values) {
				if (topScores.Count == 0 || score.RoundScore > topScores[0].RoundScore) {
					topScores.Clear();
					topScores.Add(score);
				} else if (score.RoundScore == topScores[0].RoundScore) {
					topScores.Add(score);
				}
            }

			for (int i = 0; i < topScores.Count; i++) {
				ShallowGames.scores[topScores[i].playerID].Score++;
			}

			StringBuilder sb = new StringBuilder((topScores.Count > 1 ? "Players " : "Player "));
			for (int i = 0; i < topScores.Count; i++) {
				sb.Append(string.Format("<color=#{0}>{1}</color>", ColorUtility.ToHtmlStringRGBA(PlayerManager.Instance.GetColor(topScores[i].playerID)), (topScores[i].playerID + 1)));
				if (i < topScores.Count - 2) {
					sb.Append(", ");
				} else if (i == topScores.Count - 2) {
					sb.Append(" & ");
				}
			}
			sb.Append(topScores.Count > 1 ? " win!" : " wins!");

            winnerText.gameObject.SetActive(true);
			winnerText.text = sb.ToString();

            yield return new WaitForSeconds(2.0f);
			SceneLoader.Instance.Load("Menu");
		}
	}
}