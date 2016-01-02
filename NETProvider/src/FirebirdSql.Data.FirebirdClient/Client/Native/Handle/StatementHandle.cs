﻿using FirebirdSql.Data.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirebirdSql.Data.Client.Native.Handle
{
	// public visibility added, because auto-generated assembly can't work with internal types
	public class StatementHandle : FirebirdHandle
	{
		protected override bool ReleaseHandle()
		{
			Contract.Requires(_fbClient != null);

			if (this.IsClosed)
			{
				return true;
			}

			IntPtr[] statusVector = new IntPtr[IscCodes.ISC_STATUS_LENGTH];
			StatementHandle @ref = this;
			_fbClient.isc_dsql_free_statement(statusVector, ref @ref, IscCodes.DSQL_drop);
			handle = @ref.handle;

			var exception = FesConnection.ParseStatusVector(statusVector, Charset.DefaultCharset);
			return exception == null || exception.IsWarning;
		}
	}
}