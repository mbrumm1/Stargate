export type ApiResponse<T, PropertyName extends string> = {
  success: boolean;
  message: string;
  responseCode: number;
} & { [P in PropertyName]: T }
