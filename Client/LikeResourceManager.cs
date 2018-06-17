using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace NanoChat
{
    public static class LikeResourceManager
    {
        //加速/替代资源加载
        public static Image LoadEmoji(int idxofemoji) 
        {
            Image[] indexofemoji=new Image[72];
            //indexofemoji[0] = Properties.Resources.bq__1_;
            indexofemoji[0] = Properties.Resources.bq__1_;
            indexofemoji[1] = Properties.Resources.bq__2_;
            indexofemoji[2] = Properties.Resources.bq__3_;
            indexofemoji[3] = Properties.Resources.bq__4_;
            indexofemoji[4] = Properties.Resources.bq__5_;
            indexofemoji[5] = Properties.Resources.bq__6_;
            indexofemoji[6] = Properties.Resources.bq__7_;
            indexofemoji[7] = Properties.Resources.bq__8_;
            indexofemoji[8] = Properties.Resources.bq__9_;
            indexofemoji[9] = Properties.Resources.bq__10_;
            indexofemoji[10] = Properties.Resources.bq__11_;
            indexofemoji[11] = Properties.Resources.bq__12_;
            indexofemoji[12] = Properties.Resources.bq__13_;
            indexofemoji[13] = Properties.Resources.bq__14_;
            indexofemoji[14] = Properties.Resources.bq__15_;
            indexofemoji[15] = Properties.Resources.bq__16_;
            indexofemoji[16] = Properties.Resources.bq__17_;
            indexofemoji[17] = Properties.Resources.bq__18_;
            indexofemoji[18] = Properties.Resources.bq__19_;
            indexofemoji[19] = Properties.Resources.bq__20_;
            indexofemoji[20] = Properties.Resources.bq__21_;
            indexofemoji[21] = Properties.Resources.bq__22_;
            indexofemoji[22] = Properties.Resources.bq__23_;
            indexofemoji[23] = Properties.Resources.bq__24_;
            indexofemoji[24] = Properties.Resources.bq__25_;
            indexofemoji[25] = Properties.Resources.bq__26_;
            indexofemoji[26] = Properties.Resources.bq__27_;
            indexofemoji[27] = Properties.Resources.bq__28_;
            indexofemoji[28] = Properties.Resources.bq__29_;
            indexofemoji[29] = Properties.Resources.bq__30_;
            indexofemoji[30] = Properties.Resources.bq__31_;
            indexofemoji[31] = Properties.Resources.bq__32_;
            indexofemoji[32] = Properties.Resources.bq__33_;
            indexofemoji[33] = Properties.Resources.bq__34_;
            indexofemoji[34] = Properties.Resources.bq__35_;
            indexofemoji[35] = Properties.Resources.bq__36_;
            indexofemoji[36] = Properties.Resources.bq__37_;
            indexofemoji[37] = Properties.Resources.bq__38_;
            indexofemoji[38] = Properties.Resources.bq__39_;
            indexofemoji[39] = Properties.Resources.bq__40_;
            indexofemoji[40] = Properties.Resources.bq__41_;
            indexofemoji[41] = Properties.Resources.bq__42_;
            indexofemoji[42] = Properties.Resources.bq__43_;
            indexofemoji[43] = Properties.Resources.bq__44_;
            indexofemoji[44] = Properties.Resources.bq__45_;
            indexofemoji[45] = Properties.Resources.bq__46_;
            indexofemoji[46] = Properties.Resources.bq__47_;
            indexofemoji[47] = Properties.Resources.bq__48_;
            indexofemoji[48] = Properties.Resources.bq__49_;
            indexofemoji[49] = Properties.Resources.bq__50_;
            indexofemoji[50] = Properties.Resources.bq__51_;
            indexofemoji[51] = Properties.Resources.bq__52_;
            indexofemoji[52] = Properties.Resources.bq__53_;
            indexofemoji[53] = Properties.Resources.bq__54_;
            indexofemoji[54] = Properties.Resources.bq__55_;
            indexofemoji[55] = Properties.Resources.bq__56_;
            indexofemoji[56] = Properties.Resources.bq__57_;
            indexofemoji[57] = Properties.Resources.bq__58_;
            indexofemoji[58] = Properties.Resources.bq__59_;
            indexofemoji[59] = Properties.Resources.bq__60_;
            indexofemoji[60] = Properties.Resources.bq__61_;
            indexofemoji[61] = Properties.Resources.bq__62_;
            indexofemoji[62] = Properties.Resources.bq__63_;
            indexofemoji[63] = Properties.Resources.bq__64_;
            indexofemoji[64] = Properties.Resources.bq__65_;
            indexofemoji[65] = Properties.Resources.bq__66_;
            indexofemoji[66] = Properties.Resources.bq__67_;
            indexofemoji[67] = Properties.Resources.bq__68_;
            indexofemoji[68] = Properties.Resources.bq__69_;
            indexofemoji[69] = Properties.Resources.bq__70_;
            indexofemoji[70] = Properties.Resources.bq__71_;
            indexofemoji[71] = Properties.Resources.bq__72_;
            for (int count = 0; count < 72; count++)
                if (idxofemoji - 1 == count)
                    return indexofemoji[count];
            return Properties.Resources.emoji;
            //return lastpath;
        }
    }
}
