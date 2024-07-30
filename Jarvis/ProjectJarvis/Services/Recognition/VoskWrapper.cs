using System;
using System.Runtime.InteropServices;
using System.Text;

public class VoskWrapper : IDisposable
{
    private const string VoskLib = "libvosk"; // Если библиотека vosk.dll находится в системной папке или в папке проекта

    [DllImport(VoskLib, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr vosk_model_new(string model_path);

    [DllImport(VoskLib, CallingConvention = CallingConvention.Cdecl)]
    private static extern void vosk_model_free(IntPtr model);

    [DllImport(VoskLib, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr vosk_recognizer_new(IntPtr model, float sample_rate);

    [DllImport(VoskLib, CallingConvention = CallingConvention.Cdecl)]
    private static extern void vosk_recognizer_free(IntPtr recognizer);

    [DllImport(VoskLib, CallingConvention = CallingConvention.Cdecl)]
    private static extern int vosk_recognizer_accept_waveform(IntPtr recognizer, byte[] data, int length);

    [DllImport(VoskLib, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr vosk_recognizer_result(IntPtr recognizer);

    private IntPtr model;
    private IntPtr recognizer;

    public VoskWrapper(string modelPath, float sampleRate)
    {
        model = vosk_model_new(modelPath);
        if (model == IntPtr.Zero)
        {
            throw new Exception("Failed to initialize Vosk model.");
        }

        recognizer = vosk_recognizer_new(model, sampleRate);
        if (recognizer == IntPtr.Zero)
        {
            throw new Exception("Failed to initialize Vosk recognizer.");
        }
    }

    public string Recognize(byte[] audioData)
    {
        int resultCode = vosk_recognizer_accept_waveform(recognizer, audioData, audioData.Length);
        if (resultCode != 0)
        {
            IntPtr resultPtr = vosk_recognizer_result(recognizer);
            string result = PtrToStringUtf8(resultPtr);
            return result;
        }
        return null;
    }

    private string PtrToStringUtf8(IntPtr ptr)
    {
        if (ptr == IntPtr.Zero)
        {
            return null;
        }

        // Find the length of the string in bytes
        int len = 0;
        while (Marshal.ReadByte(ptr, len) != 0)
        {
            len++;
        }

        // Allocate a byte array to hold the string
        byte[] buffer = new byte[len];
        Marshal.Copy(ptr, buffer, 0, len);

        // Convert the byte array to a string
        return Encoding.UTF8.GetString(buffer);
    }

    public void Dispose()
    {
        if (recognizer != IntPtr.Zero)
        {
            vosk_recognizer_free(recognizer);
            recognizer = IntPtr.Zero;
        }
        if (model != IntPtr.Zero)
        {
            vosk_model_free(model);
            model = IntPtr.Zero;
        }
    }
}
