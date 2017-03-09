/*
 * Created by SharpDevelop.
 * User: stanguay
 * Date: 2/7/2017
 * Time: 7:20 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace NamedPipeWrapper
{
	/// <summary>
	/// Description of MessageContainer.
	/// </summary>
	/// 
	
	[Serializable]
	public class MessageContainer
	{
		public MessageType Message {get; set; }
		
		public MessageContainer(MessageType message)
		{
			Message = message;
		}
		
		//necessary to have parameterless ctr for serialization
		public MessageContainer()
		{

		}
	}
}
