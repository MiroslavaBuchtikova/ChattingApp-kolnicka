import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { map } from 'rxjs';
import { ChatService } from 'src/chat.service';
import { Message } from 'src/message';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit {

  msgInboxArray: Message[] = [];
  form!: FormGroup;
  constructor(private chatService: ChatService, private formBuilder: FormBuilder) { }

  ngOnInit(): void {
    this.chatService.retrieveMessages().subscribe((message: Message)=>{
        let mappedMessage = new Message();
        mappedMessage.message = message.message;
        mappedMessage.user = message.user;
        this.msgInboxArray.push(mappedMessage);
    })
    this.form = this.formBuilder.group({
      username: ['', Validators.required],
      messageText: ['', Validators.required]
    })
  }

  get f(){
    return this.form.controls;
  }

  onSubmit(){
   
    if(!this.form.valid){
      console.log('Form invalid')
    }

    let message = new Message();
    message.message = this.f['messageText'].value;
    message.user = this.f['username'].value;

    this.chatService.broadcastMessage(message);
  }

}
