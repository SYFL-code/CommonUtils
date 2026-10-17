using System;
using System.Threading;

namespace CommonUtils
{
	internal sealed class OwnerGate<TOwner> where TOwner : class
	{
		private TOwner? owner;

		internal bool TryClaim(TOwner candidate)
		{
			if (candidate == null)
			{
				throw new ArgumentNullException("candidate");
			}
			TOwner? observed = Interlocked.CompareExchange<TOwner?>(ref this.owner, candidate, default(TOwner));
			return observed == null || observed == candidate;
		}

		internal bool IsOwner(TOwner candidate)
		{
			return candidate != null && Interlocked.CompareExchange<TOwner?>(ref this.owner, default(TOwner), default(TOwner)) == candidate;
		}

		internal bool TryRelease(TOwner candidate, bool cleanupComplete)
		{
			if (candidate == null)
			{
				throw new ArgumentNullException("candidate");
			}
			return cleanupComplete && Interlocked.CompareExchange<TOwner?>(ref this.owner, default(TOwner), candidate) == candidate;
		}

	}
}
