export interface ApiResponse<T> {
  success: boolean;
  message: string;
  responseCode: number;
  value: T;
}