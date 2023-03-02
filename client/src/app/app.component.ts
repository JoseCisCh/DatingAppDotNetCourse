import { Component, OnInit } from '@angular/core';
import { AccountService } from './_services/account.service';
import { User } from './_models/user'

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = "Dating app";
  users: any;

  ngOnInit(): void {
    this.setCurrentUser();
  }

  constructor(private accountService: AccountService) {}


  setCurrentUser() {
    // This complains beacause parse method cannot be sure that the value of user in local 
    // storage will be a string. It must be a string.
    // We need to override typescript with '!. Or make a explicit solution like the second one

    //const user: User = JSON.parse(localStorage.getItem('user')!);

    const userString = localStorage.getItem('user');
    if (!userString) return;

    const user: User = JSON.parse(userString);
    this.accountService.setCurrentUser(user);

  }

}
