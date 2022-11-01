using UnityEngine;
using System.Collections.Generic;


public class Grid2D : MonoBehaviour
{
	public delegate void OnReposition ();
	
	public enum Sorting
	{
		None,
		Alphabetic,
		Horizontal,
		Vertical,
		Custom,
	}

	public Sorting sorting = Sorting.None;
	public bool Decendant = false;

	public int maxPerLine = 5;

	public float cellX = 20f;
	public float cellZ = 20f;

	public bool Zigzag = false;

	public bool hideInactive = false;

	public OnReposition onReposition;

	public System.Comparison<Transform> onCustomSort;

	[HideInInspector][SerializeField] bool sorted = false;

	protected bool mReposition = false;
	protected bool mInitDone = false;

	public bool repositionNow { set { if (value) { mReposition = true; enabled = true; } } }

	[ContextMenu( "Clear Grid" )]
	public virtual void ClearGrid()
	{
		//transform.DetachChildrenn();
	}

	public List<Transform> GetChildList ()
	{
		Transform myTrans = transform;
		List<Transform> list = new List<Transform>();

		for (int i = 0; i < myTrans.childCount; ++i)
		{
			Transform t = myTrans.GetChild(i);
			if (!hideInactive || (t && t.gameObject.activeSelf))
				list.Add(t);
		}

		// Sort the list using the desired sorting logic
		if (sorting != Sorting.None )
		{
			if (sorting == Sorting.Alphabetic) list.Sort(SortByName);
			else if (sorting == Sorting.Horizontal) list.Sort(SortHorizontal);
			else if (sorting == Sorting.Vertical) list.Sort(SortVertical);
			else if (onCustomSort != null) list.Sort(onCustomSort);
			else Sort(list);
		}
		return list;
	}

	public Transform GetChild (int index)
	{
		List<Transform> list = GetChildList();
		return (index < list.Count) ? list[index] : null;
	}

	public int GetIndex (Transform trans) { return GetChildList().IndexOf(trans); }

	
	[System.Obsolete("Use gameObject.AddChild or transform.parent = gridTransform")]
	public void AddChild (Transform trans)
	{
		if (trans != null)
		{
			trans.parent = transform;
			ResetPosition(GetChildList());
		}
	}

	[System.Obsolete("Use gameObject.AddChild or transform.parent = gridTransform")]
	public void AddChild (Transform trans, bool sort)
	{
		if (trans != null)
		{
			trans.parent = transform;
			ResetPosition(GetChildList());
		}
	}

	public bool RemoveChild (Transform t)
	{
		List<Transform> list = GetChildList();

		if (list.Remove(t))
		{
			ResetPosition(list);
			return true;
		}
		return false;
	}

	protected virtual void Init ()
	{
		mInitDone = true;
	}

	protected virtual void Start ()
	{
		if (!mInitDone) Init();
		Reposition();
		enabled = false;
	}

	protected virtual void Update ()
	{
		Reposition();
		enabled = false;
	}

	public int SortByName (Transform a, Transform b) 
	{
		if(Decendant)
			return string.Compare(b.name, a.name);
		return string.Compare(a.name, b.name); 
	}
	public int SortHorizontal (Transform a, Transform b) 
	{
		if (Decendant)
			return b.localPosition.x.CompareTo(a.localPosition.x);
		return a.localPosition.x.CompareTo(b.localPosition.x); 
	}
	public int SortVertical (Transform a, Transform b) 
	{
		if (Decendant)
			return a.localPosition.y.CompareTo(b.localPosition.y);
		return b.localPosition.y.CompareTo(a.localPosition.y); 
	}

	protected virtual void Sort (List<Transform> list) { }

	[ContextMenu("Execute")]
	public virtual void Reposition ()
	{
		if (Application.isPlaying && !mInitDone ) Init();

		if (sorted)
		{
			sorted = false;
			if (sorting == Sorting.None)
				sorting = Sorting.Alphabetic;
		}

		List<Transform> list = GetChildList();
		ResetPosition(list);


		if (onReposition != null)
			onReposition();
	}

	protected virtual void ResetPosition (List<Transform> list)
	{
		mReposition = false;

		int x = 0;
		int z = 0;

		Transform myTrans = transform;
		for (int i = 0, imax = list.Count; i < imax; ++i)
		{
			Transform t = list[i];
			Vector3 pos = t.localPosition;
			float depth = pos.y;

			pos = new Vector3(cellX * x, -cellZ * z, 0f) ;
			if( Zigzag )
			{
				if( z % 2 != 0 )
					pos.x += cellX * 0.5f;
			}

			t.localPosition = pos;

			if (++x >= maxPerLine && maxPerLine > 0)
			{
				x = 0;
				++z;
			}
		}
	}
}
