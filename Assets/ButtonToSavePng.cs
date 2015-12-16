using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using UnityEngine.UI;


public class ButtonToSavePng : MonoBehaviour {
    [DllImport("PngSaveDLL")]
    private static extern int SaveTestPng();

    [DllImport("PngSaveDLL")]
    private static extern int Save16BitPng(int image_width, int image_height, byte[] image_buffer, string path);
  
    // Use this for initialization
    void Start () {
        Button button = this.GetComponent<Button>();
        button.onClick.AddListener(onClicked);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void png_save_uint_16(byte[] buf, int i, int startIndex)
    {
        buf[startIndex] = (byte)((i >> 8) & 0xff);
        buf[startIndex+1] = (byte)(i & 0xff);
    }

    void createSample16BitImage(int width, int height, byte[] imageBuffer)
    {
        float x_rate = 65535 / width;
        float y_rate = 65535 / height;

        for ( int y = 0 ; y < height ; y ++)
        {
            for (int x = 0; x < width; x ++)
            {
                int pixelPos = y * (width * 8) + x * 8;

                png_save_uint_16(imageBuffer, (int)((float)x * x_rate), pixelPos);
                png_save_uint_16(imageBuffer, (int)((float)y * y_rate), pixelPos + 2);
                png_save_uint_16(imageBuffer, 0, pixelPos + 4);
                png_save_uint_16(imageBuffer, 65535, pixelPos + 6);
            }
        }
    }

    protected virtual void onClicked()
    {
        // SaveTestPng();

        int width = 100 ;
        int height = 100 ;
        byte[] imageBuffer;
        imageBuffer = new byte[width * height * 4*2];

        for ( int i = 0; i < imageBuffer.Length; i ++)
        {
            imageBuffer[i] = 0x0;
        }

        createSample16BitImage(width, height, imageBuffer);

        Save16BitPng(width, height, imageBuffer, "test_out.png");
 
    }

}
