using UnityEngine;
using System.Collections.Generic;


//Spawn list 관리 하는 pool
public partial class GoPoolExt<T> : GoPool<T> where T : MonoBehaviour
{
	List<T> m_spwanList = new List<T>();

	public List<T> SpwanList
	{
		get
		{
			return m_spwanList;
		}
	}

	override public T Spawn()
	{
		T btn = base.Spawn();
		m_spwanList.Add( btn );
		return btn;
	}

	override public void Despawn( T go )
	{
		m_spwanList.Remove( go );
		base.Despawn( go );
	}


	override public void Clear()
	{
		m_spwanList.Clear();
		base.Clear();
	}
}

