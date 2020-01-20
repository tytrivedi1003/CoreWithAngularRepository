import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import { AlertService } from '../../services/alert/alert.service'
import { LoginService } from './../../services/login/login.service';
import { UserDetails } from './../../models/UserDetails/user-details';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  registerForm: FormGroup;
  loading = false;
  submitted = false;
  userData = new UserDetails();
  returnurl : string;
  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private loginService: LoginService,
    private alertService: AlertService
  ) { 
  }

  ngOnInit() {
    this.registerForm = this.formBuilder.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      username: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(6)]],
      mob: ['', [Validators.required]],
    });
  }
 
  get f() { return this.registerForm.controls; }

  keyPress(event: any) {
    const pattern = /[0-9\+\-\ ]/;

    let inputChar = String.fromCharCode(event.charCode);
    if (event.keyCode != 8 && !pattern.test(inputChar)) {
      event.preventDefault();
    }
  }

  onSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.registerForm.invalid) {
      return;
    }

    debugger;
    this.loading = true;
    this.userData.FirstName = this.f.firstName.value;
    this.userData.LastName = this.f.lastName.value;
    this.userData.EmailId = this.f.username.value;
    this.userData.StrPass = this.f.password.value;
    this.userData.Phone = this.f.mob.value;

    this.returnurl = './login';
    this.loginService.registerUser(this.userData)
      .pipe(first())
      .subscribe(
        data => {
          debugger;
           this.router.navigate([this.returnurl]);
        },
        error => {
          this.alertService.error(error);
          this.loading = false;
        });
  }
}
