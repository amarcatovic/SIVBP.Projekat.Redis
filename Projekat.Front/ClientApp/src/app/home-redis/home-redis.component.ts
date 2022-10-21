import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import Post from '../models/Post';

@Component({
  selector: 'app-home-redis',
  templateUrl: './home-redis.component.html'
})
export class HomeRedisComponent implements OnInit {

  baseUrl!: string;
  posts!: Post[];
  displayedColumns: string[] = ['title', 'created', 'answers', 'views'];

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  ngOnInit(): void {
    this.http.get<Post[]>(this.baseUrl + 'posts/latest/cache')
      .subscribe(result => {
        this.posts = result;
      }, error => console.error(error));
  }
}
