using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using VowelAServer.Shared.Models.Multiplayer;

public class GameRoomUI : MonoBehaviour
{
    public List<GameObject> TeamPanels;
    public GameObject TeamMemberTemplate;

    private void Start()
    {
        RoomsController.OnTeamPlayersUpdated.AddListener(TeamPlayersUpdated);
    }

    private void TeamPlayersUpdated((int teamId, List<PlayerProfile> playerProfiles) teamPlayers)
    {
        if (teamPlayers.teamId < TeamPanels.Count)
        {
            // Clear list of players in team panel
            for (var i = 0; i < TeamPanels[teamPlayers.teamId].transform.childCount; i ++)
                Destroy(TeamPanels[teamPlayers.teamId].transform.GetChild(i).gameObject);
                
            // Update list of players in team panel
            foreach (var playerProfile in teamPlayers.playerProfiles)
            {
                var teamMember = Instantiate(TeamMemberTemplate, TeamPanels[teamPlayers.teamId].transform);
                var nickname   = teamMember.GetComponentInChildren<Text>();
                nickname.text  = playerProfile.Nickname;
            }
        }
    }

    public void UpdateRoomData()     => RoomsController.RequestTeams();

    public void ChooseTeamId(int id) => RoomsController.ChooseTeamId(id);
}
