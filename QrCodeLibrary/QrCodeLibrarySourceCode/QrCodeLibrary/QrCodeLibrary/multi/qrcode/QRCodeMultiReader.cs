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
using QrCodeLibrary.multi.qrcode.detector;
using QrCodeLibrary.qrcode;

namespace QrCodeLibrary.multi.qrcode
{
	
	/// <summary> This implementation can detect and decode multiple QR Codes in an image.
	/// 
	/// </summary>
	/// <author>  Sean Owen
	/// </author>
	/// <author>  Hannes Erven
	/// </author>
	/// <author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source 
	/// </author>

	public sealed class QRCodeMultiReader:QRCodeReader, MultipleBarcodeReader
	{
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'EMPTY_RESULT_ARRAY '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private static readonly Result[] EMPTY_RESULT_ARRAY = new Result[0];
		
		public Result[] decodeMultiple(BinaryBitmap image)
		{
			return decodeMultiple(image, null);
		}

        public Result[] decodeMultiple(BinaryBitmap image, System.Collections.Generic.Dictionary<Object, Object> hints)
		{
            List<Result> results = new List<Result>(10);
			DetectorResult[] detectorResult = new MultiDetector(image.BlackMatrix).detectMulti(hints);
			for (int i = 0; i < detectorResult.Length; i++)
			{
				try
				{
					DecoderResult decoderResult = Decoder.decode(detectorResult[i].Bits);
					ResultPoint[] points = detectorResult[i].Points;
					Result result = new Result(decoderResult.Text, decoderResult.RawBytes, points, BarcodeFormat.QR_CODE);
					if (decoderResult.ByteSegments != null)
					{
						result.putMetadata(ResultMetadataType.BYTE_SEGMENTS, decoderResult.ByteSegments);
					}
					if (decoderResult.ECLevel != null)
					{
						result.putMetadata(ResultMetadataType.ERROR_CORRECTION_LEVEL, decoderResult.ECLevel.ToString());
					}
					results.Add(result);
				}
				catch (ReaderException re)
				{
					// ignore and continue 
				}
			}
			if ((results.Count == 0))
			{
				return EMPTY_RESULT_ARRAY;
			}
			else
			{
				Result[] resultArray = new Result[results.Count];
				for (int i = 0; i < results.Count; i++)
				{
					resultArray[i] = (Result) results[i];
				}
				return resultArray;
			}
		}
	}
}