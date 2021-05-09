function chkform(){
	if($('#username').val()==''){
		$('#usernametips').html('(用户名不能为空)');
		return false;
	}
	if($('#pwd').val()==''){
		$('#pwdtips').html('(密码不能为空)');
		return false;
	}
}