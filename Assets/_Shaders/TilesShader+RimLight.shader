Shader "Custom/TilesShader+RimLight"
{
	Properties
	{
		[PerRendererData]_MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		_Cutoff("Shadow alpha cutoff", Range(0,1)) = 0.5
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		 _DitherPattern ("Dithering Pattern", 2D) = "white" {}
		 _Color1 ("Dither Color 1", Color) = (0, 0, 0, 1)
        _Color2 ("Dither Color 2", Color) = (1, 1, 1, 1)
	}

	SubShader
	{
		Tags
		{
			"IgnoreProjector" = "True"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		UsePass "Custom/TilesShader/FORWARD"
		UsePass "Custom/Entities/FORWARDADD"
		//UsePass "Custom/Dither/DITHER"
	}

	FallBack "Custom/TilesShader"
}
