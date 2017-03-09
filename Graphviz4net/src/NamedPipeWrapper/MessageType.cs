/*
 * Created by SharpDevelop.
 * User: stanguay
 * Date: 2/7/2017
 * Time: 7:27 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Xml.Serialization;

namespace NamedPipeWrapper
{
	/// <summary>
	/// Description of IMessage.
	/// </summary>
	/// 

	[XmlInclude(typeof(MsgPing))]
	[XmlInclude(typeof(MsgExit))]
	[Serializable]
	public class MessageType
	{
		
	}
}
