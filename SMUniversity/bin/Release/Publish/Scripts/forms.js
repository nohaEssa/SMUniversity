"use strict";
$(function() {
    var overlay = $([
			'<!--LOGIN REGISTER RESET FORMS-->',
			'<section class="form-box-overlay"></section>'
    ].join("\n")),
		loginForm = $([
			'<!--LOGIN FORM-->',
			'<article class="form-box login">',
				'<a href="#" class="form-close">',
					'<img src="../Content/images/form-close.png" alt="close">',
				'</a>',
				'<img src="../Content/images/form-logo.png" alt="logo">',
				'<form id="login-form">',
					'<!--ERROR MSGS-->',
					'<label for="login_user" class="error">Wrong username or password...</label>',
					'<!--/ERROR MSGS-->',
					'<input type="text" name="login_user" id="login_user" placeholder="اسم المستخدم">',
					'<input type="password" name="login_pwd" id="login_pwd" placeholder="كلمة المرور">',
					'<input type="checkbox" name="login_remember" id="login_remember">',
					'<label for="login_remember">تذكرني</label>',
				'</form>',
				'<a href="#" class="popup reset">نسيت كلمة المرور?</a>',
				'<input type="submit" form="login-form" value="دخول">',
				'<p>اذا كنت لا تملك حساب سجل الان?<br><a href="#" class="popup register">تسجيل الان!</a></p>',
			'</article>',
			'<!--/LOGIN FORM-->'
		].join("\n")),
		registerForm = $([
			'<!--REGISTER FORM-->',
			'<article class="form-box register">',
				'<a href="#" class="form-close">',
					'<img src="../Content/images/form-close.png" alt="close">',
				'</a><img src="../Content/images/form-logo.png" alt="logo">',
				'<form id="register-form">',
					'<!--ERROR MSGS-->',
					'<label for="register_user" class="error">That username is already in use...</label>',
					'<!--/ERROR MSGS-->',
					'<!--OK MSGS-->',
					'<input type="text" name="register_email" id="register_email" placeholder="البريد الالكتروني">',
					'<input type="text" name="register_user" id="register_user" placeholder="اسم المستخدم">',
					'<input type="password" name="register_pwd" id="register_pwd" placeholder="كلمة المرور">',
					'<input type="password" name="register_repeat_pwd" id="register_pwd" placeholder="اعادة كلمة المرور">',
					'<input type="checkbox" name="register_terms" id="register_terms">',
					'<label for="register_terms">اوافق على الشروط والاحكام</label>',
				'</form>',
				'<a href="#">الشروط والاحكام</a>',
				'<input type="submit" form="register-form" value="تسجيل">',
				'<p>انا بالفعل املك حساب?<br><a href="#" class="popup login">دخول!</a></p>',
			'</article>',
			'<!--/REGISTER FORM-->'
		].join("\n"));
	

	// insert forms
	$('body')
		.append(overlay)
		.append(loginForm)
		.append(registerForm)
	

	// vertically center form boxes
	$('.form-box').each(function() {
		var form = $(this),
			formBoxHeight = form.outerHeight();
		form.css('marginTop', -formBoxHeight/2);
	});

	// popup open
	$('.popup').click(function(e) {
		e.preventDefault();
		var button = $(this);

		// show form overlay
		$('.form-box-overlay').fadeIn(600);
		
		// hide previous form
		button.parents('.form-box').fadeOut(600);

		// show login form and focus
		if(button.hasClass('login')) {
			$('.form-box.login').fadeIn(600);
			$('#login_user').focus();
		}
		// show register form and focus
		if(button.hasClass('register')) {
			$('.form-box.register').fadeIn(600);
			$('#register_email').focus();
		}
		if (button.hasClass('Atef')) {
		    $('.form-box.Atef').fadeIn(600);
		    $('#register_email').focus();
		}
		// show reset form and focus
		if(button.hasClass('reset')) {
			$('.form-box.reset').fadeIn(600);
			$('#reset_email').focus();
		}

		// disable scroll
		$('body').addClass('noscroll');
	});

	// popup close
	$('.form-close').click(function(e) {
		e.preventDefault();
		// close active form
		$(this).parent().fadeOut(600);

		// close form overlay
		$('.form-box-overlay').fadeOut(600);
		
		// enable scroll
		$('body').removeClass('noscroll');
	});
});