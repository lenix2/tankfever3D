﻿//Copyright 2018, Davin Carten, All rights reserved

using System.Runtime.InteropServices;

namespace emotitron.Compression
{
	/// <summary>
	/// The compressed result of crushing. Contains the individual float/quat crusher results involved, as well as the composite in the form of a Bitstream.
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	public struct CompressedElement
	{

		[FieldOffset(0)]
		public ElementCrusher crusher;

		[FieldOffset(8)]
		public uint cx;
		[FieldOffset(12)]
		public uint cy;
		[FieldOffset(16)]
		public uint cz;

		[FieldOffset(8)]
		public uint cUniform;

		[FieldOffset(8)]
		public ulong cQuat;

		[FieldOffset(8)]
		public float floatx;
		[FieldOffset(12)]
		public float floaty;
		[FieldOffset(16)]
		public float floatz;

		[FieldOffset(20)]
		public Bitstream bitstream;

		public static readonly CompressedElement Empty = new CompressedElement();

		// Constructor
		public CompressedElement(ElementCrusher crusher, CompressedValue cx, CompressedValue cy, CompressedValue cz) : this()
		{
			this.crusher = crusher;
			this.cx = cx;
			this.cy = cy;
			this.cz = cz;
			this.bitstream = new Bitstream(cx, cy, cz);
		}

		// Constructor
		public CompressedElement(ElementCrusher crusher, uint cx, uint cy, uint cz, int xbits, int ybits, int zbits) : this()
		{
			this.crusher = crusher;
			this.cx = cx;
			this.cy = cy;
			this.cz = cz;
			this.bitstream = new Bitstream(cx, xbits, cy, ybits, cz, zbits);
		}

		// Constructor for uniform scale
		public CompressedElement(ElementCrusher crusher, uint cUniform, int ubits) : this()
		{
			this.crusher = crusher;
			this.cUniform = cUniform;
			this.bitstream = new Bitstream(cUniform, ubits);
		}

		// Constructor for Quaternion rotation
		public CompressedElement(ElementCrusher crusher, ulong cQuat, int qbits) : this()
		{
			this.crusher = crusher;
			this.cQuat = cQuat;
			this.bitstream = new Bitstream(cQuat, qbits);
		}

		// Constructor
		public CompressedElement(ElementCrusher crusher, uint cx, uint cy, uint cz) : this()
		{
			this.crusher = crusher;
			this.cx = cx;
			this.cy = cy;
			this.cz = cz;
		}

		// Constructor for half-float
		public CompressedElement(ElementCrusher ec, ushort cx, ushort cy, ushort cz) : this()
		{
			this.crusher = ec;
			this.cx = cx;
			this.cy = cy;
			this.cz = cz;
			this.bitstream = new Bitstream(cx, 16, cy, 16, cz, 16);
		}

		// Constructor
		public CompressedElement(ElementCrusher ec, float x, float y, float z) : this()
		{
			this.crusher = ec;
			floatx = x;
			floaty = y;
			floatz = z;
			this.bitstream = new Bitstream(cx, 32, cy, 32, cz, 32);
		}

		// Indexer
		//TODO make these switches
		public uint this[int index]
		{
			get
			{
				return (index == 0) ? cx : (index == 1) ? cy : cz;
			}
			set
			{
				if (index == 0) cx = value;
				else if (index == 1) cy = value;
				else if (index == 2) cz = value;
			}
		}

		public float GetFloat(int axis)
		{
			return (axis == 0) ? floatx : (axis == 1) ? floaty : floatz;
		}

		public uint GetUInt(int axis)
		{
			return (axis == 0) ? cx : (axis == 1) ? cy : cz;
		}

		public static implicit operator ulong(CompressedElement val)
		{
			return val.bitstream[0];
		}

		//public static implicit operator CompressedElement(ulong val)
		//{
		//	return new CompressedElement(null, val);
		//}

		/// <summary>
		/// Basic compare of the X, Y, Z, and W values. True if they all match.
		/// </summary>
		public static bool Compare(CompressedElement a, CompressedElement b)
		{
			return (a.cx == b.cx && a.cy == b.cy && a.cz == b.cz);
		}

		public static void Copy(CompressedElement source, CompressedElement target)
		{
			target.cx = source.cx;
			target.cy = source.cy;
			target.cz = source.cz;
		}

		/// <summary>
		/// Get the bit count of the highest bit that is different between two compressed positions. This is the min number of bits that must be sent.
		/// </summary>
		/// <returns></returns>
		public static int HighestDifferentBit(uint a, uint b)
		{
			int highestDiffBit = 0;

			// TODO: This needs testing
			for (int i = 0; i < 32; i++)
			{
				uint mask = (uint)1 << i;

				if ((a & mask) == (b & mask))
				{
					highestDiffBit = i;
				}
						
			}
	

			return highestDiffBit;
		}

		public static CompressedElement operator +(CompressedElement a, CompressedElement b)
		{
			return new CompressedElement(a.crusher, (uint)((long)a.cx + b.cx), (uint)((long)a.cy + b.cy), (uint)((long)a.cz + b.cz));
		}

		public static CompressedElement operator -(CompressedElement a, CompressedElement b)
		{
			return new CompressedElement(a.crusher, (uint)((long)a.cx - b.cx), (uint)((long)a.cy - b.cy), (uint)((long)a.cz - b.cz));
		}
		public static CompressedElement operator *(CompressedElement a, float b)
		{
			return new CompressedElement(a.crusher, (uint)(a.cx * b), (uint)(a.cy * b), (uint)(a.cz * b));
		}

		//public static CompressedElement operator |(CompressedElement a, CompressedElement b)
		//{
		//	return new CompressedElement 
		//		(a.crusher,
		//		a.cx | 
		//		)
		//}

		public static CompressedElement Extrapolate(CompressedElement curr, CompressedElement prev, int divisor = 2)
		{
			return new CompressedElement
				(
				curr.crusher,
				(uint)(curr.cx + (((long)curr.cx - prev.cx)) / divisor),
				(uint)(curr.cy + (((long)curr.cy - prev.cy)) / divisor),
				(uint)(curr.cz + (((long)curr.cz - prev.cz)) / divisor)
				);
		}
		/// <summary>
		/// It is preferable to use the overload that takes and int divisor value than a float, to avoid all float math.
		/// </summary>
		public static CompressedElement Extrapolate(CompressedElement curr, CompressedElement prev, float amount = .5f)
		{
			int divisor = (int)(1f / amount);
			return Extrapolate(curr, prev, divisor);
		}

		/// <summary>
		/// Test changes between two compressed Vector3 elements and return the ideal BitCullingLevel for that change.
		/// </summary>
		public static BitCullingLevel GetGuessableBitCullLevel(CompressedElement oldComp, CompressedElement newComp, ElementCrusher ec, BitCullingLevel maxCullLvl)
		{
			for (BitCullingLevel lvl = maxCullLvl; lvl > 0; lvl--)
			{
				CompressedElement uppers = oldComp.ZeroLowerBits(ec, lvl);
				CompressedElement lowers = newComp.ZeroUpperBits(ec, lvl);

				CompressedElement merged = new CompressedElement(
					oldComp.crusher,
					uppers.cx | lowers.cx,
					uppers.cy | lowers.cy,
					uppers.cz | lowers.cz);

				// Starting guess is the new lower bits using the previous upperbits
				if (Compare(newComp, merged)) //  (oldComp.ZeroLowerBits(lvl) | newComp.ZeroUpperBits(lvl))))
					return lvl;
			}
			return BitCullingLevel.NoCulling;
		}

		/// <summary>
		/// Test changes between two compressed Vector3 elements and return the ideal BitCullingLevel for that change.
		/// </summary>
		public static BitCullingLevel GetGuessableBitCullLevel(CompressedElement oldComp, CompressedElement newComp, FloatCrusher[] ec, BitCullingLevel maxCullLvl)
		{
			for (BitCullingLevel lvl = maxCullLvl; lvl > 0; lvl--)
			{
				CompressedElement uppers = oldComp.ZeroLowerBits(ec, lvl);
				CompressedElement lowers = newComp.ZeroUpperBits(ec, lvl);

				CompressedElement merged = new CompressedElement(
					oldComp.crusher,
					uppers.cx | lowers.cx,
					uppers.cy | lowers.cy,
					uppers.cz | lowers.cz);

				// Starting guess is the new lower bits using the previous upperbits
				if (Compare(newComp, merged)) //  (oldComp.ZeroLowerBits(lvl) | newComp.ZeroUpperBits(lvl))))
					return lvl;
			}
			return BitCullingLevel.NoCulling;
		}

		/// <summary>
		/// Return the smallest bit culling level that will be able to communicate the changes between two compressed elements.
		/// </summary>
		public static BitCullingLevel FindBestBitCullLevel(CompressedElement a, CompressedElement b, ElementCrusher ec, BitCullingLevel maxCulling)
		{
			/// Quats can't cull upper bits, so its an all or nothing. Either the bits match or they don't
			if (ec.TRSType == TRSType.Quaternion)
			{
				if (a.cQuat == b.cQuat)
					return BitCullingLevel.DropAll;
				else
					return BitCullingLevel.NoCulling;
			}

			if (maxCulling == BitCullingLevel.NoCulling || !TestMatchingUpper(a, b, ec, BitCullingLevel.DropThird))
				return BitCullingLevel.NoCulling;

			if (maxCulling == BitCullingLevel.DropThird || !TestMatchingUpper(a, b, ec, BitCullingLevel.DropHalf))
				return BitCullingLevel.DropThird;

			if (maxCulling == BitCullingLevel.DropHalf || a != b)
				return BitCullingLevel.DropHalf;

			// both values are the same
			return BitCullingLevel.DropAll;
		}
		public static BitCullingLevel FindBestBitCullLevel(CompressedElement a, CompressedElement b, FloatCrusher[] ec, BitCullingLevel maxCulling)
		{
			if (maxCulling == BitCullingLevel.NoCulling || !TestMatchingUpper(a, b, ec, BitCullingLevel.DropThird))
				return BitCullingLevel.NoCulling;

			if (maxCulling == BitCullingLevel.DropThird || !TestMatchingUpper(a, b, ec, BitCullingLevel.DropHalf))
				return BitCullingLevel.DropThird;

			if (maxCulling == BitCullingLevel.DropHalf || a != b)
				return BitCullingLevel.DropHalf;

			// both values are the same
			return BitCullingLevel.DropAll;
		}

		private static bool TestMatchingUpper(uint a, uint b, int lowerbits)
		{
			return (((a >> lowerbits) << lowerbits) == ((b >> lowerbits) << lowerbits));
		}

		public static bool TestMatchingUpper(CompressedElement prevPos, CompressedElement b, ElementCrusher ar, BitCullingLevel bcl)
		{
			return
				(
				TestMatchingUpper(prevPos.cx, b.cx, ar[0].GetBitsAtCullLevel(bcl)) &&
				TestMatchingUpper(prevPos.cy, b.cy, ar[1].GetBitsAtCullLevel(bcl)) &&
				TestMatchingUpper(prevPos.cz, b.cz, ar[2].GetBitsAtCullLevel(bcl))
				);
		}
		public static bool TestMatchingUpper(CompressedElement prevPos, CompressedElement b, FloatCrusher[] ar, BitCullingLevel bcl)
		{
			return
				(
				TestMatchingUpper(prevPos.cx, b.cx, ar[0].GetBitsAtCullLevel(bcl)) &&
				TestMatchingUpper(prevPos.cy, b.cy, ar[1].GetBitsAtCullLevel(bcl)) &&
				TestMatchingUpper(prevPos.cz, b.cz, ar[2].GetBitsAtCullLevel(bcl))
				);
		}

		public override string ToString()
		{
			return "[" + cQuat + "]" + " x:" + cx + " y:" + cy + " z:" + cz;
		}
	}

	public static class CompressedElementExt
	{
		public static System.UInt32[] reusableInts = new System.UInt32[3];

		public static uint[] GetChangeAmount(CompressedElement a, CompressedElement b)
		{
			for (int i = 0; i < 3; i++)
				reusableInts[i] = (System.UInt32)System.Math.Abs(a[i] - b[0]);

			return reusableInts;
		}

		/// <summary>
		/// Alternative to OverwriteUpperBits that attempts to guess the upperbits by seeing if each axis of the new position would be
		/// closer to the old one if the upper bit is incremented by one, two, three etc. Stops trying when it fails to get a better result.
		/// </summary>
		/// <param name="oldcpos">Last best position test against.</param>
		/// <returns>Returns a corrected CompressPos</returns>
		public static CompressedElement GuessUpperBits(this CompressedElement newcpos, CompressedElement oldcpos, ElementCrusher ec, BitCullingLevel bcl)
		{
			return new CompressedElement(
				oldcpos.crusher,
				ec[0].GuessUpperBits(newcpos[0], oldcpos[0], bcl),
				ec[1].GuessUpperBits(newcpos[1], oldcpos[1], bcl),
				ec[2].GuessUpperBits(newcpos[2], oldcpos[2], bcl)
				);
		}
		public static CompressedElement GuessUpperBits(this CompressedElement newcpos, CompressedElement oldcpos, FloatCrusher[] ec, BitCullingLevel bcl)
		{
			return new CompressedElement(
				oldcpos.crusher,
				ec[0].GuessUpperBits(newcpos[0], oldcpos[0], bcl),
				ec[1].GuessUpperBits(newcpos[1], oldcpos[1], bcl),
				ec[2].GuessUpperBits(newcpos[2], oldcpos[2], bcl)
				);
		}

		/// <summary>
		/// Replace the upperbits of the first compressed element with the upper bits of the second, using BitCullingLevel as the separation point.
		/// </summary>
		public static CompressedElement OverwriteUpperBits(this CompressedElement low, CompressedElement up, ElementCrusher ec, BitCullingLevel bcl)
		{
			return new CompressedElement(
				low.crusher,
				ec[0].OverwriteUpperBits(low.cx, up.cx, bcl),
				ec[1].OverwriteUpperBits(low.cy, up.cy, bcl),
				ec[2].OverwriteUpperBits(low.cz, up.cz, bcl)
				);
		}
		public static CompressedElement OverwriteUpperBits(this CompressedElement low, CompressedElement up, FloatCrusher[] ec, BitCullingLevel bcl)
		{
			return new CompressedElement(
				low.crusher,
				ec[0].OverwriteUpperBits(low.cx, up.cx, bcl),
				ec[1].OverwriteUpperBits(low.cy, up.cy, bcl),
				ec[2].OverwriteUpperBits(low.cz, up.cz, bcl)
				);
		}

		public static CompressedElement ZeroLowerBits(this CompressedElement fullpos, ElementCrusher ec, BitCullingLevel bcl)
		{
			return new CompressedElement(
				fullpos.crusher,
				ec[0].ZeroLowerBits(fullpos.cx, bcl),
				ec[1].ZeroLowerBits(fullpos.cy, bcl),
				ec[2].ZeroLowerBits(fullpos.cz, bcl)
				);
		}
		public static CompressedElement ZeroLowerBits(this CompressedElement fullpos, FloatCrusher[] ec, BitCullingLevel bcl)
		{
			return new CompressedElement(
				fullpos.crusher,
				ec[0].ZeroLowerBits(fullpos.cx, bcl),
				ec[1].ZeroLowerBits(fullpos.cy, bcl),
				ec[2].ZeroLowerBits(fullpos.cz, bcl)
				);
		}

		public static CompressedElement ZeroUpperBits(this CompressedElement fullpos, ElementCrusher ec, BitCullingLevel bcl)
		{
			return new CompressedElement(
				fullpos.crusher,
				ec[0].ZeroUpperBits(fullpos.cx, bcl),
				ec[1].ZeroUpperBits(fullpos.cy, bcl),
				ec[2].ZeroUpperBits(fullpos.cz, bcl)
				);
		}
		public static CompressedElement ZeroUpperBits(this CompressedElement fullpos, FloatCrusher[] ec, BitCullingLevel bcl)
		{
			return new CompressedElement(
				fullpos.crusher,
				ec[0].ZeroUpperBits(fullpos.cx, bcl),
				ec[1].ZeroUpperBits(fullpos.cy, bcl),
				ec[2].ZeroUpperBits(fullpos.cz, bcl)
				);
		}

	}
}
