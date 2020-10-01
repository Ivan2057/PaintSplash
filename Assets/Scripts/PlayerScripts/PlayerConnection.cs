using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerConnection : MonoBehaviour
{
   private static NetworkConnection conn;

    public static void SetConnection(NetworkConnection connexion)
    {
        conn = connexion;
    }

    public static NetworkConnection devolverConexion()
    {
        return conn;
    }
}
