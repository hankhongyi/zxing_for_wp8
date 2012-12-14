/*
* Copyright 2009 ZXing authors
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*      http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System;
using System.Collections.Generic;
using QrCodeLibrary.common;
using QrCodeLibrary.qrcode.detector;

namespace QrCodeLibrary.multi.qrcode.detector
{
	
	/// <summary> <p>Encapsulates logic that can detect one or more QR Codes in an image, even if the QR Code
	/// is rotated or skewed, or partially obscured.</p>
	/// 
	/// </summary>
	/// <author>  Sean Owen
	/// </author>
	/// <author>  Hannes Erven
	/// </author>
	/// <author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source 
	/// </author>

	public sealed class MultiDetector:Detector
	{
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'EMPTY_DETECTOR_RESULTS '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private static readonly DetectorResult[] EMPTY_DETECTOR_RESULTS = new DetectorResult[0];
		
		public MultiDetector(BitMatrix image):base(image)
		{
		}

        public DetectorResult[] detectMulti(System.Collections.Generic.Dictionary<Object, Object> hints)
		{
			BitMatrix image = Image;
			MultiFinderPatternFinder finder = new MultiFinderPatternFinder(image);
			FinderPatternInfo[] info = finder.findMulti(hints);
			
			if (info == null || info.Length == 0)
			{
				throw ReaderException.Instance;
			}

            List<DetectorResult> result = new List<DetectorResult>(10);
			for (int i = 0; i < info.Length; i++)
			{
				try
				{
					result.Add(processFinderPatternInfo(info[i]));
				}
				catch (ReaderException e)
				{
					// ignore
				}
			}
			if ((result.Count == 0))
			{
				return EMPTY_DETECTOR_RESULTS;
			}
			else
			{
				DetectorResult[] resultArray = new DetectorResult[result.Count];
				for (int i = 0; i < result.Count; i++)
				{
					resultArray[i] = (DetectorResult) result[i];
				}
				return resultArray;
			}
		}
	}
}