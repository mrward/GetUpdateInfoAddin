//
// MyClass.cs
//
// Author:
//       Matt Ward <matt.ward@microsoft.com>
//
// Copyright (c) 2019 Microsoft
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System.Collections.Generic;
using System.Linq;
using Mono.Addins;
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Updater;
using System.Text;

namespace GetUpdateInfoAddin
{
	class GetUpdateInfoHandler : CommandHandler
	{
		protected override void Run ()
		{
			var invalidUpdateInfos = new List<ProductInformationProvider> ();
			var list = new List<UpdateInfo> ();

			var installedProducts =
				AddinManager.GetExtensionObjects<ISystemInformationProvider> ("/MonoDevelop/Core/SystemInformation", false)
					.OfType<ProductInformationProvider> ();

			foreach (var product in installedProducts) {
				var uinfo = product.GetUpdateInfo ();
				if (uinfo == null) {
					invalidUpdateInfos.Add (product);
				} else {
					list.Add (uinfo);
				}
			}

			if (invalidUpdateInfos.Count > 0) {
				var message = new StringBuilder ();
				foreach (var product in invalidUpdateInfos) {
					message.AppendLine (string.Format ("  {0} {1}", product.Title, product.Version));
				}
				MessageService.ShowWarning ("Missing update info for installed products:", message.ToString ());
			} else {
				MessageService.ShowMessage ("No missing update info for installed products");
			}
		}
	}
}
