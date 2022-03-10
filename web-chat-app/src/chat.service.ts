import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Observable } from 'rxjs';
import { Subject } from 'rxjs/internal/Subject';
import { Message } from './message';

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  chatMessage! : Message;
  sharedObj = new Subject<Message>();
  private receivedMessageObject: Message = new Message();

  private connection: any = new signalR.HubConnectionBuilder()
  .withUrl('https://localhost:7082/chatsocket', 
  {skipNegotiation: true, transport: signalR.HttpTransportType.WebSockets})
  .configureLogging(signalR.LogLevel.Information)
  .build();

  constructor(private http: HttpClient) {
    console.log('start chat service')
    this.connection.onclose(async()=>{
      await this.start();
    });

  this.connection.on('SendMessage', (message: Message)=>{
        console.log(message)
        this.mapReceivedMessage(message.user!, message.message!);
    });

    this.start();

   }
  public async start() {
    try{
      await this.connection.start();
      console.log('connected')

    }
    catch(err){
      console.log(err);
      setTimeout(()=> this.start(), 5000);
    }
  }
  private mapReceivedMessage(user: string, message: string): void {
    this.receivedMessageObject.user = user;
    this.receivedMessageObject.message = message;
    this.sharedObj.next(this.receivedMessageObject);
  }

  public retrieveMessages(): Observable<Message>{
    return this.sharedObj.asObservable();
  }

  public broadcastMessage(message: Message){
    this.http.post('https://localhost:7082/api/Chat', message).subscribe((data)=> console.log(data));
  }
}
