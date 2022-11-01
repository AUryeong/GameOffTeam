using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public partial class DelayedCall : MonoSingleton<DelayedCall>
{
	GoPoolExt<DelayedCallStub> stubPool = new GoPoolExt<DelayedCallStub>();

	override protected void Awake()
	{
		base.Awake();
		stubPool.SetCreater( CreateStub );
	}

	DelayedCallStub GetByTag( object tag )
	{
		foreach( DelayedCallStub stub in stubPool.SpwanList )
		{
			if( stub.enabled )
			{
				DelayedCallStub obj = stub.GetComponent<DelayedCallStub>();
				if( obj != null && obj._tag != null )
				{
					if( obj._tag.Equals( tag ) )
						return stub;
				}
			}
		}

		return null;
	}

	DelayedCallStub CreateStub()
	{
		GameObject go = new GameObject();
		go.name = stubPool.Count.ToString();
		go.transform.parent = DelayedCall.Get().transform;
		return go.AddComponent<DelayedCallStub>();
	}

	public void onFrameCountCall<T>( int nCount, System.Action<T> func, T data, object tag = null )
	{
		DelayedCallStub obj = stubPool.Spawn();
		obj.enabled = true;
		obj._tag = tag;
		obj._forStop = DelayedCall.instance.StartCoroutine( obj.FrameCountCall( func, data, nCount ) );
	}

	public void onFrameCountCall( int nCount, System.Action func, object tag = null )
	{
		DelayedCallStub obj = stubPool.Spawn();
		obj.enabled = true;
		obj._tag = tag;
		obj._forStop = DelayedCall.instance.StartCoroutine( obj.FrameCountCall( func, nCount ) );
	}


	public void onFixedUpdateCall<T>( System.Action<T> func, T data, object tag = null )
	{
		DelayedCallStub obj = stubPool.Spawn();
		obj.enabled = true;
		obj._tag = tag;
		obj._forStop = DelayedCall.instance.StartCoroutine( obj.onFixedUpdateCall( func, data ) );
	}

	public void onFixedUpdateCall( System.Action func, object tag = null )
	{
		DelayedCallStub obj = stubPool.Spawn();
		obj.enabled = true;
		obj._tag = tag;
		obj._forStop = DelayedCall.instance.StartCoroutine( obj.onFixedUpdateCall( func ) );
	}

	public void call<T>( float sec, System.Action<T> func, T data, object tag = null )
	{
		DelayedCallStub obj = stubPool.Spawn();
		obj.enabled = true;
		obj._tag = tag;
		obj._forStop = DelayedCall.instance.StartCoroutine( obj.OnTime( sec, func, data ) );
	}

	public void call( float sec, System.Action func, object tag = null )
	{
		DelayedCallStub obj = stubPool.Spawn();
		obj.enabled = true;
		obj._tag = tag;
		obj._forStop = DelayedCall.instance.StartCoroutine( obj.OnTime( sec, func ) );
	}
	public void callRealTime(float sec, System.Action func, object tag = null)
	{
		DelayedCallStub obj = stubPool.Spawn();
		obj.enabled = true;
		obj._tag = tag;
		obj._forStop = DelayedCall.instance.StartCoroutine(obj.OnRealtime(sec, func));
	}

	public bool removeDelayCallByTag( object tag )
	{
		DelayedCallStub obj = GetByTag( tag );

		if( obj != null )
		{
			if( obj._forStop != null )
			{
				DelayedCall.instance.StopCoroutine( obj._forStop );
				obj._forStop = null;
			}
			obj.DestroyImmed();
			return true;
		}

		return false;
	}
}




