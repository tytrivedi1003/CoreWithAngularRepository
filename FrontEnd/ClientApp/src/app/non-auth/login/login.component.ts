import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import { AlertService } from '../../services/alert/alert.service'
import { LoginService }from './../../services/login/login.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  returnUrl: string;
  loading = false;
  submitted = false;

  constructor(private formBuilder: FormBuilder,
              private loginService: LoginService,
              private router: Router,
              private route: ActivatedRoute,
              private alertService: AlertService){
  }

  ngOnInit() {
    this.loginForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });

    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }

  get f() { return this.loginForm.controls; }

  onSubmit() {
      this.submitted = true;

      if (this.loginForm.invalid) {
        return;
      }

      this.loading = true;
      this.loginService.loginUser(this.f.username.value, this.f.password.value)
      .pipe(first())
      .subscribe(
          data => {
              debugger;
              this.router.navigate([this.returnUrl]);
          },
          error => {
              this.alertService.error(error);
              this.loading = false;
          });

    }
}
