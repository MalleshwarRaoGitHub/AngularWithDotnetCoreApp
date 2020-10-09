import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { connectableObservableDescriptor } from 'rxjs/internal/observable/ConnectableObservable';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  model: any = {};


  constructor(private authservice: AuthService, private alertify: AlertifyService, private router: Router) { }

  ngOnInit() {
  };
  
  login(){
    this.authservice.login(this.model).subscribe(next=>{
      this.alertify.success('logged in successfully');
    }, error => {
      this.alertify.error('failed to login');
    }, () =>{
      this.router.navigate(['/members'])}
    );
  }

  loggedIn(){
    return this.authservice.loggedIn();
  }

  logOut(){
     localStorage.removeItem('token');
     this.alertify.message('logged Out');  
     this.router.navigate(['/home']);
  }
}
