using System;

[Serializable]
public class MyFacebookInfo
{
	public string id="";
	public string name="";
	public string accesstoken="";
	public string email="";
	public Picture picture;
}

[Serializable]
public class PictureData
{
	public bool is_silhouette ;
	public string url ;
}

[Serializable]
public class Picture
{
	public PictureData data ;
}