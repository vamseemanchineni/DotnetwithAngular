import { Component, OnInit } from '@angular/core';
import { MessageService } from '../_services/message.service';
import { Message } from '../_models/message';
import { Pagination } from '../_models/pagination';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {

  messages?: Message[];
  pagination? : Pagination;
  container = 'Unread';
  pageNumber =1;
  pageSize=5;
  loading = false;
  

  constructor(private messageSerive : MessageService) {}

  ngOnInit(): void {
    this.loadMessages();
  }

  loadMessages()
  {
    this.loading=true;
    this.messageSerive.getMessages(this.pageNumber, this.pageSize, this.container).subscribe({
      next : response => {
        this.messages = response.result;
        this.pagination = response.pagination;
        this.loading = false;
      }
    })
  }

  deleteMessage(id : number)
  {
    this.messageSerive.deleteMessage(id).subscribe({
      next: () => this.messages?.splice(this.messages.findIndex(m=>m.id === id), 1 )
    })
  }

  pageChanged(event : any) {
    if(this.pageNumber != event.page){
      this.pageNumber = event.page
      this.loadMessages();
    }
  }

}
