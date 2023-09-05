import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { Observable, of } from 'rxjs';
import { User } from '../_models/user';
import { Route, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit{
  model : any = {};
 // CurrentUser$ : Observable<User | null> = of(null);

  constructor(public accountService : AccountService, private router: Router, private toastr: ToastrService) {}
  ngOnInit(): void {
    //this.CurrentUser$ = this.accountService.currentUser$;
  }

  login(){
    this.accountService.login(this.model).subscribe({
      next:() =>this.router.navigateByUrl('/members'),
      error: error => this.toastr.error(error.error)
    })
  }

  logout(){
    this.router.navigateByUrl('/')
    this.accountService.logout();
  }

}
