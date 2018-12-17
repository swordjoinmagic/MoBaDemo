using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script {
    class MyBitConverter {
        public static int ToInt32(byte[] bytes, int start) {
            byte[] tempData = new byte[4];
            for (int i=start+3;i>=start;i--) {
                tempData[-i+start+3] = bytes[i];
            }
            return BitConverter.ToInt32(tempData,0);
        }
        public static float ToFloat(byte[] bytes, int start) {
            byte[] tempData = new byte[4];
            for (int i = start + 3; i >= start; i--) {
                tempData[-i + start + 3] = bytes[i];
            }
            return BitConverter.ToSingle(tempData, 0);
        }
    }
}
