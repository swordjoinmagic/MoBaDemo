using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class RoomPlayer {
    private string userName;
    private string userFaction;

    public string UserName {
        get {
            return userName;
        }

        set {
            userName = value;
        }
    }

    public string UserFaction {
        get {
            return userFaction;
        }

        set {
            userFaction = value;
        }
    }
}

