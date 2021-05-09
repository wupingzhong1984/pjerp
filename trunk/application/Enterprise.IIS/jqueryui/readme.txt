修改原代码记录：

修改日期：2013-08-16
修改文件：js/jquery-ui-1.9.2.custom.js & js/jquery-ui-1.9.2.custom.min.js
修改原因：在弹出层中输入框的下拉自动完成层不能遮罩文字
修改内容：代码1844行
	      .zIndex( this.element.zIndex() + 1 );
		  改为
		  .zIndex( this.element.zIndex() + 10 );