import { Injectable } from "@angular/core";
import { Observable } from 'rxjs';
import { Subject } from 'rxjs';
import { map } from "rxjs/operators";
import { WebsocketService } from "./websocket.service";
import { Post } from '../models/Post'

const WS_URL = "wss://localhost:44306/ws";

@Injectable()
export class UpdateService {
  public messages: Subject<Post>;

  constructor(wsService: WebsocketService) {
    this.messages = <Subject<Post>>wsService.connect(WS_URL).pipe(
      map((response: MessageEvent): Post => {
        let data = JSON.parse(response.data);
        return {
          Id: data.Id,
          Title: data.Title,
          Description: data.Description,
          Url: data.Url,
          ImageUrl: data.ImageUrl
        }; 
      })
    );
  }
}