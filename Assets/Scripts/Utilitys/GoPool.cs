using UnityEngine;
using System.Collections.Generic;


public class GoPoolContainer
{
	static public List<PoolInterface> pools = new List<PoolInterface>();

	static public void Clear()
	{
		for( int i = 0; i < pools.Count; i++ )
		{
			pools[i].Clear();
		}
	}
}


public interface PoolInterface
{
	void Clear();
}
public partial class GoPool<T> : PoolInterface where T : MonoBehaviour  
{
	static public List<object> pools = new List<object>();

	static private Queue<T> _Pool = new Queue<T>();

	public delegate T CreateGo();
	private CreateGo _createGo;
	public CreateGo createGo
	{
		get
		{
			return _createGo;
		}
	}

	public void SetCreater( CreateGo creater)
	{
		_createGo = creater;
		if( GoPoolContainer.pools.Contains( this ) == false )
			GoPoolContainer.pools.Add( this );
	}

	public int Count
	{
		get
		{
			return _Pool.Count;
		}
	}

	public Queue<T> Pool
	{
		get
		{
			return _Pool;
		}
	}

	virtual public T Spawn()
	{
		T btn = default(T);
		if( _Pool.Count > 0 )
			btn = _Pool.Dequeue();
		else
		{
			btn = _createGo();
		}
		return btn;
	}

	virtual public void Despawn( T go )
	{
		_Pool.Enqueue( go );
	}


	virtual public void Clear()
	{
		foreach( T go in _Pool )
		{
			try
			{
				GameObject.Destroy( go.gameObject );
			}
			catch
			{
				LMJ.LogError("GoPool clear error : " + typeof( T ).Name );
			}
		}
		_Pool.Clear();
	}

	//public void Destroy()
	//{
	//	foreach( T go in _Pool )
	//	{
	//		GameObject.Destroy( go.gameObject );
	//	}
	//	Clear();
	//	EventManager.removeEventListenerByTag( GetHashCode() );
	//}
}

