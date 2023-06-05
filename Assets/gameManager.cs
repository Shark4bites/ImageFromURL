using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
	// script reference: https://www.youtube.com/watch?v=WiSLbSZKG_k
	
	public Image imgOutput; // Image output
	public InputField urlInput; // Url input
	
	public void buttonClick()
	{
		StartCoroutine(GetImgFromWeb(urlInput.text));
	}
	
	IEnumerator GetImgFromWeb(string url)
	{
		UnityWebRequest req = UnityWebRequestTexture.GetTexture(url); // Download img from url
		yield return req.SendWebRequest(); // Begin communicating
		
		if(req.isNetworkError || req.isHttpError)
		{
			// Issue from internet, or link
			Debug.Log(req.error);
		}
		else
		{
			// If no error
			Texture2D img = ((DownloadHandlerTexture)req.downloadHandler).texture; // Extract img to texture
			float imgW = img.width; // Img Width
			float imgH = img.height; // Img Height
			imgOutput.sprite = Sprite.Create(img, new Rect(0, 0, imgW, imgH), Vector2.zero); // Texture to Sprite
			
			float imgLim = 200f;
			// Get n Set new Img ratio, if still above Limit
			while (imgH > imgLim || imgW > imgLim)
			{
				if(imgH > imgLim) 
				{
					imgW = imgW/imgH*imgLim;
					imgH = imgLim;
				}
				else
				{
					imgH = imgH/imgW*imgLim;
					imgW = imgLim;
				}
			}
			// Rescale imgOutput
			imgOutput.rectTransform.sizeDelta = new Vector2(imgW, imgH);
		}
	}
}

/* Note from Elvin 1 (Devlog)
I never make code to download img from URL, so I need to look for tutorials
On Friday night I was able to find a tutorial, https://www.youtube.com/watch?v=KSoFIqxJSCc
But after finishing it, there are some features still missing from requirements
			
On Monday, I found a better tutorial with flexible inputs, https://www.youtube.com/watch?v=WiSLbSZKG_k 
But did not explain how to rescale image or compress large image
So I think on my own how to rescale image first: 1) Set value for new size, 2) Apply new value*/
			
/* Note from Elvin 2 (Improvements, Compress Large Image)
I researched and found byte[] is easier to compress large image
I found a way to download img as byte [], https://stackoverflow.com/questions/4599686/image-to-byte-array-from-a-url
I also found a way for byte [] -> sprite, https://docs.unity3d.com/ScriptReference/ImageConversion.LoadImage.html 
But I am unfamiliar with WebClient (required for download img as Byte[]), and no tutorial for it
Without tutorial/guideline for me to follow, I find it risky to try this alone*/
