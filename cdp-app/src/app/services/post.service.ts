import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Post } from '../models/Post'
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PostService {

  postsUrl:string = 'http://wasd.xyz'

  constructor(private http:HttpClient) { }

  getPostsUrl():Observable<Post[]> {
    return this.http.get<Post[]>(this.postsUrl);
  }

  getPosts() {
    return [
      {
        id: "000",
        title: "A meteorite struck the garden",
        description: "",
          imgurl: "https://img-9gag-fun.9cache.com/photo/aj5wVwQ_460swp.webp",
        url: "https://9gag.com/gag/aj5wVwQ"
      },
      {
        id: "001",
        title: "Baby polar bear practicing its balancing",
        description: "",
        imgurl: "https://img-9gag-fun.9cache.com/photo/ap5w9wE_460swp.webp",
        url: "https://9gag.com/gag/ap5w9wE"
      },
      {
        id: "002",
        title: "Evil creatures probably would have stolen the car if they had thumbs",
        description: "",
        imgurl: "https://img-9gag-fun.9cache.com/photo/a85OqQ3_460swp.webp",
        url: "https://9gag.com/gag/a85OqQ3"
      }
    ]
  }
}
