using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mirror.Examples.NetworkRoom
{
 /*   [AddComponentMenu("")]
    public class NetworkRoomManagerExt : NetworkRoomManager
    {
        /// <summary>
        /// Called just after GamePlayer object is instantiated and just before it replaces RoomPlayer object.
        /// This is the ideal point to pass any data like player name, credentials, tokens, colors, etc.
        /// into the GamePlayer object as it is about to enter the Online scene.
        /// </summary>
        /// <param name="roomPlayer"></param>
        /// <param name="gamePlayer"></param><DirectedGraph xmlns="http://schemas.microsoft.com/vs/2009/dgml">
  <Nodes>
    <Node Id="(@1 @2)" Visibility="Hidden" />
    <Node Id="(@3 Namespace=Mirror.Examples.NetworkRoom Type=NetworkRoomManagerExt)" Category="CodeSchema_Class" CodeSchemaProperty_IsPublic="True" CommonLabel="NetworkRoomManagerExt" Icon="Microsoft.VisualStudio.Class.Public" IsDragSource="True" Label="NetworkRoomManagerExt" SourceLocation="(Assembly=file:///D:/Documentos/Proyectos/Unity/Proyects/PaintSplash/Assets/Mirror/Examples/Room/Scripts/NetworkRoomManagerExt.cs StartLineNumber=6 StartCharacterOffset=17 EndLineNumber=6 EndCharacterOffset=38)" />
  </Nodes>
  <Links>
    <Link Source="(@1 @2)" Target="(@3 Namespace=Mirror.Examples.NetworkRoom Type=NetworkRoomManagerExt)" Category="Contains" />
  </Links>
  <Categories>
    <Category Id="CodeSchema_Class" Label="Clase" BasedOn="CodeSchema_Type" Icon="CodeSchema_Class" />
    <Category Id="CodeSchema_Type" Label="Tipo" Icon="CodeSchema_Class" />
    <Category Id="Contains" Label="Contiene" Description="Indica si el origen del vínculo contiene el objeto de destino." IsContainment="True" />
  </Categories>
  <Properties>
    <Property Id="CodeSchemaProperty_IsPublic" Label="Público" Description="Marca para indicar que el ámbito es Public." DataType="System.Boolean" />
    <Property Id="CommonLabel" DataType="System.String" />
    <Property Id="Icon" Label="Icono" DataType="System.String" />
    <Property Id="IsContainment" DataType="System.Boolean" />
    <Property Id="IsDragSource" Label="IsDragSource" Description="IsDragSource" DataType="System.Boolean" />
    <Property Id="Label" Label="Etiqueta" Description="Etiqueta Displayable de un objeto Annotatable." DataType="System.String" />
    <Property Id="SourceLocation" Label="Número de línea de inicio" DataType="Microsoft.VisualStudio.GraphModel.CodeSchema.SourceLocation" />
    <Property Id="Visibility" Label="Visibilidad" Description="Define si un nodo del gráfico está visible o no." DataType="System.Windows.Visibility" />
  </Properties>
  <QualifiedNames>
    <Name Id="Assembly" Label="Ensamblado" ValueType="Uri" />
    <Name Id="File" Label="Archivo" ValueType="Uri" />
    <Name Id="Namespace" Label="Espacio de nombres" ValueType="System.String" />
    <Name Id="Type" Label="Tipo" ValueType="System.Object" />
  </QualifiedNames>
  <IdentifierAliases>
    <Alias n="1" Uri="Assembly=$(VsSolutionUri)/Mirror.Examples.csproj" />
    <Alias n="2" Uri="File=$(VsSolutionUri)/Assets/Mirror/Examples/Room/Scripts/NetworkRoomManagerExt.cs" />
    <Alias n="3" Uri="Assembly=$(f48562d6-c754-1e4e-a3c1-c084e1e26c12.OutputPathUri)" />
  </IdentifierAliases>
  <Paths>
    <Path Id="f48562d6-c754-1e4e-a3c1-c084e1e26c12.OutputPathUri" Value="file:///D:/Documentos/Proyectos/Unity/Proyects/PaintSplash/Temp/bin/Debug/Mirror.Examples.dll" />
    <Path Id="VsSolutionUri" Value="file:///D:/Documentos/Proyectos/Unity/Proyects/PaintSplash" />
  </Paths>
</DirectedGraph>
        /// <returns>true unless some code in here decides it needs to abort the replacement</returns>
        public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnection conn, GameObject roomPlayer, GameObject gamePlayer)
        {
            PlayerScore playerScore = gamePlayer.GetComponent<PlayerScore>();
            playerScore.index = roomPlayer.GetComponent<NetworkRoomPlayer>().index;
            return true;
        }

        public override void OnRoomStopClient()
        {
            // Demonstrates how to get the Network Manager out of DontDestroyOnLoad when
            // going to the offline scene to avoid collision with the one that lives there.
            if (gameObject.scene.name == "DontDestroyOnLoad" && !string.IsNullOrEmpty(offlineScene) && SceneManager.GetActiveScene().path != offlineScene)
                SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());

            base.OnRoomStopClient();
        }

        public override void OnRoomStopServer()
        {
            // Demonstrates how to get the Network Manager out of DontDestroyOnLoad when
            // going to the offline scene to avoid collision with the one that lives there.
            if (gameObject.scene.name == "DontDestroyOnLoad" && !string.IsNullOrEmpty(offlineScene) && SceneManager.GetActiveScene().path != offlineScene)
                SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());

            base.OnRoomStopServer();
        }

        /*
            This code below is to demonstrate how to do a Start button that only appears for the Host player
            showStartButton is a local bool that's needed because OnRoomServerPlayersReady is only fired when
            all players are ready, but if a player cancels their ready state there's no callback to set it back to false
            Therefore, allPlayersReady is used in combination with showStartButton to show/hide the Start button correctly.
            Setting showStartButton false when the button is pressed hides it in the game scene since NetworkRoomManager
            is set as DontDestroyOnLoad = true.
        */
 /*
        bool showStartButton;

        public override void OnRoomServerPlayersReady()
        {
            // calling the base method calls ServerChangeScene as soon as all players are in Ready state.
            if (isHeadless)
                base.OnRoomServerPlayersReady();
            else
                showStartButton = true;
        }

        public override void OnGUI()
        {
            base.OnGUI();

            if (allPlayersReady && showStartButton && GUI.Button(new Rect(150, 300, 120, 20), "START GAME"))
            {
                // set to false to hide it in the game scene
                showStartButton = false;

                ServerChangeScene(GameplayScene);
            }
        }
    }*/
}
