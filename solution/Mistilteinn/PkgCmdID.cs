// PkgCmdID.cs
// MUST match PkgCmdID.h
using System;

namespace Mistilteinn
{
    static class PkgCmdIDList
    {
        public const int cmdidFixup = 0x0101;
        public const int cmdidMasterize = 0x0102;
        public const int cmdidTicketList = 0x0103;
        public const int cmdidPrivateBuild = 0x0104;
        public const int cmdidPull = 0x0105;

        public const int cmdidTaskCombo = 0x0201;
        public const int cmdidTaskComboGetList = 0x0202;
    };
}