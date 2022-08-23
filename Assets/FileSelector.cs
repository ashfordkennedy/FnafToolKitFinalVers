
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Windows;
using System.Runtime.InteropServices;



[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]

public class OpenFileName
{
    public int structSize = 0;
    public IntPtr dlgOwner = IntPtr.Zero;
    public IntPtr instance = IntPtr.Zero;
    public String filter = null;
    public String customFilter = null;
    public int maxCustFilter = 0;
    public int filterIndex = 0;
    public String file = null;
    public int maxFile = 0;
    public String fileTitle = null;
    public int maxFileTitle = 0;
    public String initialDir = null;
    public String title = null;
    public int flags = 0;
    public short fileOffset = 0;
    public short fileExtension = 0;
    public String defExt = null;
    public IntPtr custData = IntPtr.Zero;
    public IntPtr hook = IntPtr.Zero;
    public String templateName = null;
    public IntPtr reservedPtr = IntPtr.Zero;
    public int reservedInt = 0;
    public int flagsEx = 0;
}

public class DllTest
{
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetOpenFileName([In, Out] OpenFileName ofn);
    public static bool GetOpenFileName1([In, Out] OpenFileName ofn)
    {
        return GetOpenFileName(ofn);
    }

    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetSaveFileName([In, Out] OpenFileName ofn);

    public static bool GetSaveFileName1([In, Out] OpenFileName ofn)
    {
        return GetSaveFileName(ofn);
    }

   // [DllImport("Forms.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
   // public static extern bool Get
}

public class FileSelector : MonoBehaviour
{



    public static bool SelectFile(out OpenFileName fileData, string fileType = "png", string menuMessage = "", string filterMessage = "")
    {
       
        Debug.Log("running open file");
        OpenFileName ofn = new OpenFileName();
        ofn.structSize = Marshal.SizeOf(ofn);
        ofn.filter = string.Format("{1}\0*.{0}\0\0", fileType, filterMessage);
        ofn.file = new string(new char[256]);
        ofn.maxFile = ofn.file.Length;
        ofn.fileTitle = new string(new char[64]);
        ofn.maxFileTitle = ofn.fileTitle.Length;
        ofn.initialDir = UnityEngine.Application.persistentDataPath;
        ofn.title = menuMessage;
        ofn.defExt = fileType;
        ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;//OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR
        

        if (DllTest.GetOpenFileName(ofn))
        {
            Debug.Log("Selected file with full path: {0}" + ofn.file);
            fileData = ofn;
            return true;
        }
        else
        {
            fileData = new OpenFileName();
            fileData.title = "";
           return false;
        }

    }

    public static bool SelectSaveFile(out OpenFileName fileData, string fileType = "png", string menuMessage = "", string filterMessage = "")
    {

        Debug.Log("running open file");
        OpenFileName ofn = new OpenFileName();
        ofn.structSize = Marshal.SizeOf(ofn);
        ofn.filter = string.Format("{1}\0*.{0}\0\0", fileType, filterMessage);
        ofn.file = new string(new char[256]);
        ofn.maxFile = ofn.file.Length;
        ofn.fileTitle = new string(new char[64]);
        ofn.maxFileTitle = ofn.fileTitle.Length;
        ofn.initialDir = UnityEngine.Application.persistentDataPath;
        ofn.title = menuMessage;
        ofn.defExt = fileType;
        ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;//OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR
        int testflag = 23 | 32453 | 34532 | 232;

  
        if (DllTest.GetSaveFileName(ofn))
        {
            Debug.Log("Selected file with full path: {0}" + ofn.file);
            fileData = ofn;
            return true;
        }
        else
        {
            fileData = new OpenFileName();
            fileData.title = "";
            return false;
        }

    }

}
