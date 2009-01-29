﻿#region license
// Copyright (c) 2007-2009 Mauricio Scheffer
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using NUnit.Framework;
using Rhino.Mocks;
using SolrNet.Commands;
using SolrNet.Commands.Parameters;

namespace SolrNet.Tests {
	[TestFixture]
	public class DeleteCommandTests {
		[Test]
		public void DeleteById() {
			const string id = "123123";
			var mocks = new MockRepository();
			var conn = mocks.CreateMock<ISolrConnection>();
			With.Mocks(mocks).Expecting(delegate {
				Expect.Call(conn.Post("/update", string.Format("<delete><id>{0}</id></delete>", id))).Repeat.Once().Return("");
			}).Verify(delegate {
				var cmd = new DeleteCommand(new DeleteByIdParam(id));
				cmd.Execute(conn);
			});
		}

		[Test]
		public void DeleteByQuery() {
			var mocks = new MockRepository();
			var conn = mocks.CreateMock<ISolrConnection>();
			var q = mocks.CreateMock<ISolrQuery>();
			With.Mocks(mocks).Expecting(delegate {
				const string queryString = "someQuery";
				Expect.Call(q.Query).Repeat.Once().Return(queryString);
				Expect.Call(conn.Post("/update", string.Format("<delete><query>{0}</query></delete>", queryString))).Repeat.Once().
					Return("");
			}).Verify(delegate {
				var cmd = new DeleteCommand(new DeleteByQueryParam(q));
				cmd.Execute(conn);
			});
		}

		[Test]
		public void DeleteFromCommitted() {
			const string id = "123123";
			var mocks = new MockRepository();
			var conn = mocks.CreateMock<ISolrConnection>();
			With.Mocks(mocks).Expecting(delegate {
				Expect.Call(conn.Post("/update", string.Format("<delete fromCommitted=\"true\"><id>{0}</id></delete>", id))).Repeat.Once().Return("");
			}).Verify(delegate {
				var cmd = new DeleteCommand(new DeleteByIdParam(id)) {FromCommitted = true};
				cmd.Execute(conn);
			});
		}

		[Test]
		public void DeleteFromCommittedAndFromPending() {
			const string id = "123123";
			var mocks = new MockRepository();
			var conn = mocks.CreateMock<ISolrConnection>();
			With.Mocks(mocks).Expecting(delegate {
				Expect.Call(conn.Post("/update", string.Format("<delete fromPending=\"false\" fromCommitted=\"false\"><id>{0}</id></delete>", id))).Repeat.Once().Return("");
			}).Verify(delegate {
				var cmd = new DeleteCommand(new DeleteByIdParam(id)) {FromCommitted = false, FromPending = false};
				cmd.Execute(conn);
			});
		}

		[Test]
		public void DeleteFromPending() {
			const string id = "123123";
			var mocks = new MockRepository();
			var conn = mocks.CreateMock<ISolrConnection>();
			With.Mocks(mocks).Expecting(delegate {
				Expect.Call(conn.Post("/update", string.Format("<delete fromPending=\"true\"><id>{0}</id></delete>", id))).Repeat.Once().Return("");
			}).Verify(delegate {
				var cmd = new DeleteCommand(new DeleteByIdParam(id)) {FromPending = true};
				cmd.Execute(conn);
			});
		}
	}
}