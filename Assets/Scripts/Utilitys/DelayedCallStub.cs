using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class DelayedCall : MonoSingleton<DelayedCall>
{
	class DelayedCallStub : MonoBehaviour
	{
		public Coroutine _forStop;
		public object _tag;

		int _nframeCount = 0;

		void Update()
		{
			_nframeCount++;
		}

		public void DestroyImmed()
		{
			_forStop = null;
			_tag = null;

			_nframeCount = 0;

			DelayedCall.Get().stubPool.Despawn( this );
			enabled = false;
		}

		public IEnumerator FrameCountCall<T>( System.Action<T> func, T data, int nCount )
		{
			while( true )
			{
				yield return new WaitForEndOfFrame();
				_nframeCount++;
				if( _nframeCount >= nCount )
				{
					DestroyImmed();
					func( data );
					break;
				}
			}
		}

		public IEnumerator FrameCountCall( System.Action func, int nCount )
		{
			while( true )
			{
				yield return new WaitForEndOfFrame();
				_nframeCount++;
				if( _nframeCount >= nCount )
				{
					DestroyImmed();
					func();
					break;
				}
			}
		}

		//---wait fixed---------------------------------------------------------------------------
		public IEnumerator onFixedUpdateCall<T>( System.Action<T> func, T data )
		{
			yield return new WaitForFixedUpdate();
			DestroyImmed();
			func( data );
		}

		public IEnumerator onFixedUpdateCall( System.Action func )
		{
			yield return new WaitForFixedUpdate();
			DestroyImmed();
			func();
		}


		//------------------------------------------------------------------------------
		public IEnumerator OnTime<T>( float fsec, System.Action<T> func, T data )
		{
			yield return new WaitForSeconds( fsec );
			DestroyImmed();
			func( data );

		}

		public IEnumerator OnTime( float fsec, System.Action func )
		{
			yield return new WaitForSeconds( fsec );
			DestroyImmed();
			func();
		}
		public IEnumerator OnRealtime(float fsec, System.Action func)
		{
			yield return new WaitForSecondsRealtime(fsec);
			DestroyImmed();
			func();
		}
	}
}
	