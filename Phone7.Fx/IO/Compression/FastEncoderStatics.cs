namespace Phone7.Fx.IO.Compression
{
    internal static class FastEncoderStatics
    {
        // static information for encoding, DO NOT MODIFY

        internal static readonly byte[] FastEncoderTreeStructureData = { 
            0xed,0xbd,0x07,0x60,0x1c,0x49,0x96,0x25,0x26,0x2f,0x6d,0xca,
            0x7b,0x7f,0x4a,0xf5,0x4a,0xd7,0xe0,0x74,0xa1,0x08,0x80,0x60, 
            0x13,0x24,0xd8,0x90,0x40,0x10,0xec,0xc1,0x88,0xcd,0xe6,0x92, 
            0xec,0x1d,0x69,0x47,0x23,0x29,0xab,0x2a,0x81,0xca,0x65,0x56,
            0x65,0x5d,0x66,0x16,0x40,0xcc,0xed,0x9d,0xbc,0xf7,0xde,0x7b, 
            0xef,0xbd,0xf7,0xde,0x7b,0xef,0xbd,0xf7,0xba,0x3b,0x9d,0x4e,
            0x27,0xf7,0xdf,0xff,0x3f,0x5c,0x66,0x64,0x01,0x6c,0xf6,0xce,
            0x4a,0xda,0xc9,0x9e,0x21,0x80,0xaa,0xc8,0x1f,0x3f,0x7e,0x7c,
            0x1f,0x3f, 
        };

        // Output a currentMatch with length matchLen (>= MIN_MATCH) and displacement matchPos 
        //
        // Optimisation: unlike the other encoders, here we have an array of codes for each currentMatch 
        // length (not just each currentMatch length slot), complete with all the extra bits filled in, in
        // a single array element.
        //
        // There are many advantages to doing this: 
        //
        // 1. A single array lookup on g_FastEncoderLiteralCodeInfo, instead of separate array lookups 
        //    on g_LengthLookup (to get the length slot), g_FastEncoderLiteralTreeLength, 
        //    g_FastEncoderLiteralTreeCode, g_ExtraLengthBits, and g_BitMask
        // 
        // 2. The array is an array of ULONGs, so no access penalty, unlike for accessing those USHORT
        //    code arrays in the other encoders (although they could be made into ULONGs with some
        //    modifications to the source).
        // 
        // Note, if we could guarantee that codeLen <= 16 always, then we could skip an if statement here.
        // 
        // A completely different optimisation is used for the distance codes since, obviously, a table for 
        // all 8192 distances combining their extra bits is not feasible.  The distance codeinfo table is
        // made up of code[], len[] and # extraBits for this code. 
        //
        // The advantages are similar to the above; a ULONG array instead of a USHORT and BYTE array, better
        // cache locality, fewer memory operations.
        // 


        // Encoding information for literal and Length. 
        // The least 5 significant bits are the length
        // and the rest is the code bits. 

        internal static readonly uint[] FastEncoderLiteralCodeInfo = {
            0x0000d7ee,0x0004d7ee,0x0002d7ee,0x0006d7ee,0x0001d7ee,0x0005d7ee,0x0003d7ee,
            0x0007d7ee,0x000037ee,0x0000c7ec,0x00000126,0x000437ee,0x000237ee,0x000637ee, 
            0x000137ee,0x000537ee,0x000337ee,0x000737ee,0x0000b7ee,0x0004b7ee,0x0002b7ee,
            0x0006b7ee,0x0001b7ee,0x0005b7ee,0x0003b7ee,0x0007b7ee,0x000077ee,0x000477ee, 
            0x000277ee,0x000677ee,0x000017ed,0x000177ee,0x00000526,0x000577ee,0x000023ea, 
            0x0001c7ec,0x000377ee,0x000777ee,0x000217ed,0x000063ea,0x00000b68,0x00000ee9,
            0x00005beb,0x000013ea,0x00000467,0x00001b68,0x00000c67,0x00002ee9,0x00000768, 
            0x00001768,0x00000f68,0x00001ee9,0x00001f68,0x00003ee9,0x000053ea,0x000001e9,
            0x000000e8,0x000021e9,0x000011e9,0x000010e8,0x000031e9,0x000033ea,0x000008e8,
            0x0000f7ee,0x0004f7ee,0x000018e8,0x000009e9,0x000004e8,0x000029e9,0x000014e8,
            0x000019e9,0x000073ea,0x0000dbeb,0x00000ce8,0x00003beb,0x0002f7ee,0x000039e9, 
            0x00000bea,0x000005e9,0x00004bea,0x000025e9,0x000027ec,0x000015e9,0x000035e9,
            0x00000de9,0x00002bea,0x000127ec,0x0000bbeb,0x0006f7ee,0x0001f7ee,0x0000a7ec, 
            0x00007beb,0x0005f7ee,0x0000fbeb,0x0003f7ee,0x0007f7ee,0x00000fee,0x00000326, 
            0x00000267,0x00000a67,0x00000667,0x00000726,0x00001ce8,0x000002e8,0x00000e67,
            0x000000a6,0x0001a7ec,0x00002de9,0x000004a6,0x00000167,0x00000967,0x000002a6, 
            0x00000567,0x000117ed,0x000006a6,0x000001a6,0x000005a6,0x00000d67,0x000012e8,
            0x00000ae8,0x00001de9,0x00001ae8,0x000007eb,0x000317ed,0x000067ec,0x000097ed,
            0x000297ed,0x00040fee,0x00020fee,0x00060fee,0x00010fee,0x00050fee,0x00030fee,
            0x00070fee,0x00008fee,0x00048fee,0x00028fee,0x00068fee,0x00018fee,0x00058fee, 
            0x00038fee,0x00078fee,0x00004fee,0x00044fee,0x00024fee,0x00064fee,0x00014fee,
            0x00054fee,0x00034fee,0x00074fee,0x0000cfee,0x0004cfee,0x0002cfee,0x0006cfee, 
            0x0001cfee,0x0005cfee,0x0003cfee,0x0007cfee,0x00002fee,0x00042fee,0x00022fee, 
            0x00062fee,0x00012fee,0x00052fee,0x00032fee,0x00072fee,0x0000afee,0x0004afee,
            0x0002afee,0x0006afee,0x0001afee,0x0005afee,0x0003afee,0x0007afee,0x00006fee, 
            0x00046fee,0x00026fee,0x00066fee,0x00016fee,0x00056fee,0x00036fee,0x00076fee,
            0x0000efee,0x0004efee,0x0002efee,0x0006efee,0x0001efee,0x0005efee,0x0003efee,
            0x0007efee,0x00001fee,0x00041fee,0x00021fee,0x00061fee,0x00011fee,0x00051fee,
            0x00031fee,0x00071fee,0x00009fee,0x00049fee,0x00029fee,0x00069fee,0x00019fee, 
            0x00059fee,0x00039fee,0x00079fee,0x00005fee,0x00045fee,0x00025fee,0x00065fee,
            0x00015fee,0x00055fee,0x00035fee,0x00075fee,0x0000dfee,0x0004dfee,0x0002dfee, 
            0x0006dfee,0x0001dfee,0x0005dfee,0x0003dfee,0x0007dfee,0x00003fee,0x00043fee, 
            0x00023fee,0x00063fee,0x00013fee,0x00053fee,0x00033fee,0x00073fee,0x0000bfee,
            0x0004bfee,0x0002bfee,0x0006bfee,0x0001bfee,0x0005bfee,0x0003bfee,0x0007bfee, 
            0x00007fee,0x00047fee,0x00027fee,0x00067fee,0x00017fee,0x000197ed,0x000397ed,
            0x000057ed,0x00057fee,0x000257ed,0x00037fee,0x000157ed,0x00077fee,0x000357ed,
            0x0000ffee,0x0004ffee,0x0002ffee,0x0006ffee,0x0001ffee,0x00000084,0x00000003,
            0x00000184,0x00000044,0x00000144,0x000000c5,0x000002c5,0x000001c5,0x000003c6, 
            0x000007c6,0x00000026,0x00000426,0x000003a7,0x00000ba7,0x000007a7,0x00000fa7,
            0x00000227,0x00000627,0x00000a27,0x00000e27,0x00000068,0x00000868,0x00001068, 
            0x00001868,0x00000369,0x00001369,0x00002369,0x00003369,0x000006ea,0x000026ea, 
            0x000046ea,0x000066ea,0x000016eb,0x000036eb,0x000056eb,0x000076eb,0x000096eb,
            0x0000b6eb,0x0000d6eb,0x0000f6eb,0x00003dec,0x00007dec,0x0000bdec,0x0000fdec, 
            0x00013dec,0x00017dec,0x0001bdec,0x0001fdec,0x00006bed,0x0000ebed,0x00016bed,
            0x0001ebed,0x00026bed,0x0002ebed,0x00036bed,0x0003ebed,0x000003ec,0x000043ec,
            0x000083ec,0x0000c3ec,0x000103ec,0x000143ec,0x000183ec,0x0001c3ec,0x00001bee,
            0x00009bee,0x00011bee,0x00019bee,0x00021bee,0x00029bee,0x00031bee,0x00039bee, 
            0x00041bee,0x00049bee,0x00051bee,0x00059bee,0x00061bee,0x00069bee,0x00071bee,
            0x00079bee,0x000167f0,0x000367f0,0x000567f0,0x000767f0,0x000967f0,0x000b67f0, 
            0x000d67f0,0x000f67f0,0x001167f0,0x001367f0,0x001567f0,0x001767f0,0x001967f0, 
            0x001b67f0,0x001d67f0,0x001f67f0,0x000087ef,0x000187ef,0x000287ef,0x000387ef,
            0x000487ef,0x000587ef,0x000687ef,0x000787ef,0x000887ef,0x000987ef,0x000a87ef, 
            0x000b87ef,0x000c87ef,0x000d87ef,0x000e87ef,0x000f87ef,0x0000e7f0,0x0002e7f0,
            0x0004e7f0,0x0006e7f0,0x0008e7f0,0x000ae7f0,0x000ce7f0,0x000ee7f0,0x0010e7f0,
            0x0012e7f0,0x0014e7f0,0x0016e7f0,0x0018e7f0,0x001ae7f0,0x001ce7f0,0x001ee7f0,
            0x0005fff3,0x000dfff3,0x0015fff3,0x001dfff3,0x0025fff3,0x002dfff3,0x0035fff3, 
            0x003dfff3,0x0045fff3,0x004dfff3,0x0055fff3,0x005dfff3,0x0065fff3,0x006dfff3,
            0x0075fff3,0x007dfff3,0x0085fff3,0x008dfff3,0x0095fff3,0x009dfff3,0x00a5fff3, 
            0x00adfff3,0x00b5fff3,0x00bdfff3,0x00c5fff3,0x00cdfff3,0x00d5fff3,0x00ddfff3, 
            0x00e5fff3,0x00edfff3,0x00f5fff3,0x00fdfff3,0x0003fff3,0x000bfff3,0x0013fff3,
            0x001bfff3,0x0023fff3,0x002bfff3,0x0033fff3,0x003bfff3,0x0043fff3,0x004bfff3, 
            0x0053fff3,0x005bfff3,0x0063fff3,0x006bfff3,0x0073fff3,0x007bfff3,0x0083fff3,
            0x008bfff3,0x0093fff3,0x009bfff3,0x00a3fff3,0x00abfff3,0x00b3fff3,0x00bbfff3,
            0x00c3fff3,0x00cbfff3,0x00d3fff3,0x00dbfff3,0x00e3fff3,0x00ebfff3,0x00f3fff3,
            0x00fbfff3,0x0007fff3,0x000ffff3,0x0017fff3,0x001ffff3,0x0027fff3,0x002ffff3, 
            0x0037fff3,0x003ffff3,0x0047fff3,0x004ffff3,0x0057fff3,0x005ffff3,0x0067fff3,
            0x006ffff3,0x0077fff3,0x007ffff3,0x0087fff3,0x008ffff3,0x0097fff3,0x009ffff3, 
            0x00a7fff3,0x00affff3,0x00b7fff3,0x00bffff3,0x00c7fff3,0x00cffff3,0x00d7fff3, 
            0x00dffff3,0x00e7fff3,0x00effff3,0x00f7fff3,0x00fffff3,0x0001e7f1,0x0003e7f1,
            0x0005e7f1,0x0007e7f1,0x0009e7f1,0x000be7f1,0x000de7f1,0x000fe7f1,0x0011e7f1, 
            0x0013e7f1,0x0015e7f1,0x0017e7f1,0x0019e7f1,0x001be7f1,0x001de7f1,0x001fe7f1,
            0x0021e7f1,0x0023e7f1,0x0025e7f1,0x0027e7f1,0x0029e7f1,0x002be7f1,0x002de7f1,
            0x002fe7f1,0x0031e7f1,0x0033e7f1,0x0035e7f1,0x0037e7f1,0x0039e7f1,0x003be7f1,
            0x003de7f1,0x000047eb, 
        };

        internal static readonly uint[] FastEncoderDistanceCodeInfo = { 
            0x00000f06,0x0001ff0a,0x0003ff0b,0x0007ff0b,0x0000ff19,0x00003f18,0x0000bf28,
            0x00007f28,0x00001f37,0x00005f37,0x00000d45,0x00002f46,0x00000054,0x00001d55, 
            0x00000864,0x00000365,0x00000474,0x00001375,0x00000c84,0x00000284,0x00000a94,
            0x00000694,0x00000ea4,0x000001a4,0x000009b4,0x00000bb5,0x000005c4,0x00001bc5,
            0x000007d5,0x000017d5,0x00000000,0x00000100,
        };

        internal static readonly uint[] BitMask = { 0, 1, 3, 7, 15, 31, 63, 127, 255, 511, 1023, 2047, 4095, 8191, 16383, 32767 };
        internal static readonly byte[] ExtraLengthBits = { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 0 };
        internal static readonly byte[] ExtraDistanceBits = { 0, 0, 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10, 11, 11, 12, 12, 13, 13, 0, 0 };
        internal const int NumChars = 256;
        internal const int NumLengthBaseCodes = 29;
        internal const int NumDistBaseCodes = 30;


    }
}