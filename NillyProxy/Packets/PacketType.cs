﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NillyProxy.Packets
{
    public enum PacketType
    {
        UNKNOWN,
        FAILURE,
        CREATESUCCESS,
        CREATE,
        PLAYERSHOOT,
        MOVE,
        PLAYERTEXT,
        TEXT,
        SHOOT2,
        DAMAGE,
        UPDATE,
        UPDATEACK,
        NOTIFICATION,
        NEWTICK,
        INVSWAP,
        USEITEM,
        SHOWEFFECT,
        HELLO,
        GOTO,
        INVDROP,
        INVRESULT,
        RECONNECT,
        PING,
        PONG,
        MAPINFO,
        LOAD,
        PIC,
        SETCONDITION,
        TELEPORT,
        USEPORTAL,
        DEATH,
        BUY,
        BUYRESULT,
        AOE,
        GROUNDDAMAGE,
        PLAYERHIT,
        ENEMYHIT,
        AOEACK,
        SHOOTACK,
        OTHERHIT,
        SQUAREHIT,
        GOTOACK,
        EDITACCOUNTLIST,
        ACCOUNTLIST,
        QUESTOBJID,
        CHOOSENAME,
        NAMERESULT,
        CREATEGUILD,
        CREATEGUILDRESULT,
        GUILDREMOVE,
        GUILDINVITE,
        ALLYSHOOT,
        SHOOT,
        REQUESTTRADE,
        TRADEREQUESTED,
        TRADESTART,
        CHANGETRADE,
        TRADECHANGED,
        ACCEPTTRADE,
        CANCELTRADE,
        TRADEDONE,
        TRADEACCEPTED,
        CLIENTSTAT,
        CHECKCREDITS,
        ESCAPE,
        FILE,
        INVITEDTOGUILD,
        JOINGUILD,
        CHANGEGUILDRANK,
        PLAYSOUND,
        GLOBALNOTIFICATION,
        RESKIN,
        ENTERARENA,
        LEAVEARENA,
        PETCOMMAND,
        PETYARDCOMMAND,
        TINKERQUEST,
        VIEWQUESTS,
        ARENADEATH,
        ARENANEXTWAVE,
        HATCHEGG,
        NEWABILITYUNLOCKED,
        PASSWORDPROMPT,
        EVOLVEPET,
        QUESTFETCHRESPONSE,
        REMOVEPET,
        UPDATEPET,
        UPGRADEPETYARDRESULT,
        VERIFYEMAILDIALOG,
        QUESTREDEEMRESPONSE,
        CHEATING
    }
}
