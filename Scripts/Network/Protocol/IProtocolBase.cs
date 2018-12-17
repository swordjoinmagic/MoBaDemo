using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IProtocolBase {
    byte[] Encode();
    string Name {
        get;
    }
    string Desc {
        get;
    }

}

