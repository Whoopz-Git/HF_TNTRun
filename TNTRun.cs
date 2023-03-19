using System.Collections;
using HoldfastSharedMethods;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class TNTRun : IHoldfastSharedMethods, IHoldfastSharedMethods2
{
    public class Players
    {
        public int playerId;
        public GameObject playerObj;
    }

    private InputField f1MenuInputField;
    private int roundStartTime = 480;
    private List<int> alivePlayerCount = new List<int>();
    private bool startRound = false;
    private bool findCubes = true;

    private List<GameObject> cubes = new List<GameObject>();
    private List<Players> playerList = new List<Players>();
    private Vector3 playerPos;
    private Vector3 cubePos;

    public void findAllCubes()
    {
        var allCubes = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "Cube");
        cubes = allCubes.ToList();

        findCubes = false;
        Debug.Log(cubes.Count + " cubes found");
    }



    public async void searchPositions()
    {
        for (int x = 0; x < cubes.Count; x++)
        {
            for (int y = 0; y < playerList.Count; y++)
            {
                playerPos = roundVector((Vector3)playerList[y].playerObj.transform.position);
                cubePos = roundVector((Vector3)cubes[x].transform.position);
                if (Vector3.Distance(playerPos, cubePos) < 0.2)
                {
                    await Task.Delay(350);
                    cubes[x].SetActive(false);
                    continue;
                }
            }

        }
    }


    public Vector3 roundVector(Vector3 vector3)
    {
        return new Vector3( 
            Mathf.Floor(vector3.x),
            Mathf.Floor(vector3.y),
            Mathf.Floor(vector3.z));
    }

    public void OnIsClient(bool client, ulong steamId)
    {
        if (findCubes)
        {
            findAllCubes();
        }
    }

    public void OnIsServer(bool server)
    {
        //Get all the canvas items in the game
        if (findCubes)
        {
            findAllCubes();
        }

        var canvases = Resources.FindObjectsOfTypeAll<Canvas>();
        for (int i = 0; i < canvases.Length; i++)
        {
            //Find the one that's called "Game Console Panel"
            if (string.Compare(canvases[i].name, "Game Console Panel", true) == 0)
            {
                //Inside this, now we need to find the input field where the player types messages.
                f1MenuInputField = canvases[i].GetComponentInChildren<InputField>(true);
                if (f1MenuInputField != null)
                {
                    Debug.Log("Found the Game Console Panel");
                }
                else
                {
                    Debug.Log("We did Not find Game Console Panel");
                }
                break;
            }
        }
    }

    public void OnUpdateTimeRemaining(float time)
    {
        if (time > roundStartTime + 1)
            return;

        if (((int)time == roundStartTime) && (alivePlayerCount.Count > 1))
        {
            startRound = true;
            Debug.Log("Round enabled");
        }

        if (startRound)
        {
            searchPositions();

            if (alivePlayerCount.Count == 1)
            {
                Debug.Log("Round won by " + alivePlayerCount[0]);
                var endRound = string.Format("set roundEndPlayerWin {0}", alivePlayerCount[0]);
                f1MenuInputField.onEndEdit.Invoke(endRound);
                startRound = false;
            }
            else
            {
                return;
            }
        }

        return;

    }

    public void OnPlayerHurt(int playerId, byte oldHp, byte newHp, EntityHealthChangedReason reason)
    {
        if (alivePlayerCount.Contains(playerId))
        {
            alivePlayerCount.Remove(playerId);
            Debug.Log("Player Removed (dead). Count is now " + alivePlayerCount.Count);
        }

        for (int x = 0; x < playerList.Count; x++)
        {
            if (playerList[x].playerId == playerId)
            {
                playerList.Remove(playerList[x]);
                Debug.Log("Player Removed (dead). Count is now " + playerList.Count);
            }
        }
    }

    public void OnPlayerLeft(int playerId)
    {
        if (alivePlayerCount.Contains(playerId))
        {
            alivePlayerCount.Remove(playerId);
            Debug.Log("Player removed (left). Count is now " + playerList.Count);
        }

        for (int x = 0; x < playerList.Count; x++)
        {
            if(playerList[x].playerId == playerId)
            {
                playerList.Remove(playerList[x]);
            }
        }
    }

    public void OnPlayerSpawned(int playerId, int spawnSectionId, FactionCountry playerFaction, PlayerClass playerClass, int uniformId, GameObject playerObject)
    {
        Players newplayer = new Players();
        newplayer.playerId = playerId;
        newplayer.playerObj = playerObject;
        alivePlayerCount.Add(playerId);
        playerList.Add(newplayer);
        Debug.Log("Player added. Count is now " + alivePlayerCount.Count);
    }


    public void OnTextMessage(int playerId, TextChatChannel channel, string text)
    {

    }

    public void PassConfigVariables(string[] value)
    {

    }

    public void OnPlayerJoined(int playerId, ulong steamId, string playerName, string regimentTag, bool isBot)
    {

    }

    public void OnSyncValueState(int value)
    {

    }

    public void OnUpdateSyncedTime(double time)
    {

    }

    public void OnUpdateElapsedTime(float time)
    {

    }


    public void OnPlayerPacket(int playerId, byte? instance, Vector3? ownerPosition, double? packetTimestamp, Vector2? ownerInputAxis, float? ownerRotationY, float? ownerPitch, float? ownerYaw, PlayerActions[] actionCollection, Vector3? cameraPosition, Vector3? cameraForward, ushort? shipID, bool swimming)
    {
    }

    public void OnDamageableObjectDamaged(GameObject damageableObject, int damageableObjectId, int shipId, int oldHp, int newHp)
    {

    }

    public void OnPlayerKilledPlayer(int killerPlayerId, int victimPlayerId, EntityHealthChangedReason reason, string additionalDetails)
    {

    }

    public void OnPlayerShoot(int playerId, bool dryShot)
    {

    }

    public void OnScorableAction(int playerId, int score, ScorableActionType reason)
    {

    }

    public void OnRoundDetails(int roundId, string serverName, string mapName, FactionCountry attackingFaction, FactionCountry defendingFaction, GameplayMode gameplayMode, GameType gameType)
    {

    }

    public void OnPlayerBlock(int attackingPlayerId, int defendingPlayerId)
    {

    }

    public void OnPlayerMeleeStartSecondaryAttack(int playerId)
    {

    }

    public void OnPlayerWeaponSwitch(int playerId, string weapon)
    {

    }

    public void OnCapturePointCaptured(int capturePoint)
    {

    }

    public void OnCapturePointOwnerChanged(int capturePoint, FactionCountry factionCountry)
    {

    }

    public void OnCapturePointDataUpdated(int capturePoint, int defendingPlayerCount, int attackingPlayerCount)
    {

    }

    public void OnRoundEndFactionWinner(FactionCountry factionCountry, FactionRoundWinnerReason reason)
    {

    }

    public void OnRoundEndPlayerWinner(int playerId)
    {

    }

    public void OnPlayerStartCarry(int playerId, CarryableObjectType carryableObject)
    {

    }

    public void OnPlayerEndCarry(int playerId)
    {

    }

    public void OnPlayerShout(int playerId, CharacterVoicePhrase voicePhrase)
    {

    }

    public void OnInteractableObjectInteraction(int playerId, int interactableObjectId, GameObject interactableObject, InteractionActivationType interactionActivationType, int nextActivationStateTransitionIndex)
    {

    }

    public void OnEmplacementPlaced(int itemId, GameObject objectBuilt, EmplacementType emplacementType)
    {

    }

    public void OnEmplacementConstructed(int itemId)
    {

    }

    public void OnBuffStart(int playerId, BuffType buff)
    {

    }

    public void OnBuffStop(int playerId, BuffType buff)
    {

    }

    public void OnShotInfo(int playerId, int shotCount, Vector3[][] shotsPointsPositions, float[] trajectileDistances, float[] distanceFromFiringPositions, float[] horizontalDeviationAngles, float[] maxHorizontalDeviationAngles, float[] muzzleVelocities, float[] gravities, float[] damageHitBaseDamages, float[] damageRangeUnitValues, float[] damagePostTraitAndBuffValues, float[] totalDamages, Vector3[] hitPositions, Vector3[] hitDirections, int[] hitPlayerIds, int[] hitDamageableObjectIds, int[] hitShipIds, int[] hitVehicleIds)
    {

    }

    public void OnVehicleSpawned(int vehicleId, FactionCountry vehicleFaction, PlayerClass vehicleClass, GameObject vehicleObject, int ownerPlayerId)
    {

    }

    public void OnVehicleHurt(int vehicleId, byte oldHp, byte newHp, EntityHealthChangedReason reason)
    {

    }

    public void OnPlayerKilledVehicle(int killerPlayerId, int victimVehicleId, EntityHealthChangedReason reason, string details)
    {

    }

    public void OnShipSpawned(int shipId, GameObject shipObject, FactionCountry shipfaction, ShipType shipType, int shipNameId)
    {

    }

    public void OnShipDamaged(int shipId, int oldHp, int newHp)
    {

    }

    public void OnAdminPlayerAction(int playerId, int adminId, ServerAdminAction action, string reason)
    {

    }

    public void OnConsoleCommand(string input, string output, bool success)
    {

    }

    public void OnRCLogin(int playerId, string inputPassword, bool isLoggedIn)
    {

    }

    public void OnRCCommand(int playerId, string input, string output, bool success)
    {

    }

    public void OnVehiclePacket(int vehicleId, Vector2 inputAxis, bool shift, bool strafe, PlayerVehicleActions[] actionCollection)
    {

    }

    public void OnOfficerOrderStart(int officerPlayerId, HighCommandOrderType highCommandOrderType, Vector3 orderPosition, float orderRotationY, int voicePhraseRandomIndex)
    {

    }

    public void OnOfficerOrderStop(int officerPlayerId, HighCommandOrderType highCommandOrderType)
    {

    }
}
