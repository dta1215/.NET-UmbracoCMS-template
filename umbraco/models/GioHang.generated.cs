//------------------------------------------------------------------------------
// <auto-generated>
//   This code was generated by a tool.
//
//    Umbraco.ModelsBuilder.Embedded v10.0.1+fd0c4fd
//
//   Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Linq.Expressions;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Infrastructure.ModelsBuilder;
using Umbraco.Cms.Core;
using Umbraco.Extensions;

namespace Umbraco.Cms.Web.Common.PublishedModels
{
	/// <summary>Giỏ hàng</summary>
	[PublishedModel("gioHang")]
	public partial class GioHang : PublishedContentModel
	{
		// helpers
#pragma warning disable 0109 // new is redundant
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.0.1+fd0c4fd")]
		public new const string ModelTypeAlias = "gioHang";
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.0.1+fd0c4fd")]
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.0.1+fd0c4fd")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public new static IPublishedContentType GetModelContentType(IPublishedSnapshotAccessor publishedSnapshotAccessor)
			=> PublishedModelUtility.GetModelContentType(publishedSnapshotAccessor, ModelItemType, ModelTypeAlias);
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.0.1+fd0c4fd")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public static IPublishedPropertyType GetModelPropertyType<TValue>(IPublishedSnapshotAccessor publishedSnapshotAccessor, Expression<Func<GioHang, TValue>> selector)
			=> PublishedModelUtility.GetModelPropertyType(GetModelContentType(publishedSnapshotAccessor), selector);
#pragma warning restore 0109

		private IPublishedValueFallback _publishedValueFallback;

		// ctor
		public GioHang(IPublishedContent content, IPublishedValueFallback publishedValueFallback)
			: base(content, publishedValueFallback)
		{
			_publishedValueFallback = publishedValueFallback;
		}

		// properties

		///<summary>
		/// Giỏ hàng_danh sách sản phẩm đã mua
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.0.1+fd0c4fd")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("gioHang_danhSachSanPhamDaMua")]
		public virtual global::System.Collections.Generic.IEnumerable<global::Umbraco.Cms.Core.Models.PublishedContent.IPublishedContent> GioHang_danhSachSanPhamDaMua => this.Value<global::System.Collections.Generic.IEnumerable<global::Umbraco.Cms.Core.Models.PublishedContent.IPublishedContent>>(_publishedValueFallback, "gioHang_danhSachSanPhamDaMua");

		///<summary>
		/// Giỏ hàng_địa chỉ
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.0.1+fd0c4fd")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("gioHang_diaChi")]
		public virtual string GioHang_diaChi => this.Value<string>(_publishedValueFallback, "gioHang_diaChi");

		///<summary>
		/// Giỏ hàng_ngày thanh toán
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.0.1+fd0c4fd")]
		[ImplementPropertyType("gioHang_ngayThanhToan")]
		public virtual global::System.DateTime GioHang_ngayThanhToan => this.Value<global::System.DateTime>(_publishedValueFallback, "gioHang_ngayThanhToan");

		///<summary>
		/// Giỏ hàng_Người thanh toán
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.0.1+fd0c4fd")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("gioHang_NguoiThanhToan")]
		public virtual string GioHang_NguoiThanhToan => this.Value<string>(_publishedValueFallback, "gioHang_NguoiThanhToan");

		///<summary>
		/// Giỏ hàng_số lượng
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.0.1+fd0c4fd")]
		[ImplementPropertyType("gioHang_soLuong")]
		public virtual int GioHang_soLuong => this.Value<int>(_publishedValueFallback, "gioHang_soLuong");

		///<summary>
		/// Giỏ hàng_tổng tiền
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.0.1+fd0c4fd")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("gioHang_tongTien")]
		public virtual string GioHang_tongTien => this.Value<string>(_publishedValueFallback, "gioHang_tongTien");

		///<summary>
		/// Giỏ hàng_trạng thái
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.0.1+fd0c4fd")]
		[ImplementPropertyType("gioHang_trangThai")]
		public virtual bool GioHang_trangThai => this.Value<bool>(_publishedValueFallback, "gioHang_trangThai");
	}
}
